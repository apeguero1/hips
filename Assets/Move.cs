using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class Move : MonoBehaviour
{
    // Vector3 random_vel(float a,float b){
    //     return new Vector3(Random.Range(a,b),
    //         Random.Range(a,b),0);
    // }


    public GameObject Block;
    float var1;

    //List<GameObject> npcs = new List<GameObject>();
    //List<Rigidbody> rb_npcs = new List<Rigidbody>();
    List<GameObject> agents = new List<GameObject>();

    List<GameObject> players = new List<GameObject>();
    enum AgentState
    {
        Stationary,
        Walking,
        Running
    }

    //var agentStates = new List<AgentState>();
    Dictionary<int,AgentState> agentStates = new Dictionary<int,AgentState>();

    //List<physics> npcs = new List<GameObject>();

    int N_agent = 10;
    int N_player = 2;
    
    

    // Start is called before the first frame update
    
    float[,] T = new float[2, 2];
    //int p1 = Random.Range(0,N_npc);
    void Start()
    {
        //print("AgentState.Stationary: " + (int) AgentState.Stationary);

        List<int> playersI = new List<int>();
        for (int n=0; n<N_player; n++){
            var i = Random.Range(0,N_agent);
            while (playersI.Contains(i)) {
                i = Random.Range(0,N_agent);
            }
            playersI.Add(i);
        }
         
        print("players "+string.Join(", ",playersI));

        for (int n=0; n<N_agent; n++)
            {
                //Random.Range(-10.0f, 10.0f);
                /*npcs.Add(Instantiate(Block2, new Vector3(Random.Range(-Global.X/2,Global.X/2),
                                              Random.Range(-Global.Y/2,Global.Y/2),0), 
                                              Quaternion.identity)
                */

                
                
                var agent = Instantiate(Block, new Vector3(-Global.X/2,
                                              -Global.Y/2 + n*Global.Y/N_agent   ,0), 
                                              Quaternion.identity);

                agent.AddComponent<Rigidbody>();
                
                agent.GetComponent<Collider>().enabled = false;
                agent.GetComponent<Rigidbody>().useGravity = false;

                agent.transform.localScale = new Vector3 (1, 1, 1)*0.5f;
                agentStates.Add(agent.GetInstanceID(),AgentState.Stationary);
                
                if (playersI.Contains(n) ){
                    players.Add(agent);
                    }
                else{
                    agents.Add(agent);
                }



            }

        T[(int)AgentState.Stationary, (int)AgentState.Stationary] = 0.97f;
        T[(int)AgentState.Stationary, (int)AgentState.Walking] = 1f - T[(int)AgentState.Stationary, (int)AgentState.Stationary];
        T[(int)AgentState.Walking, (int)AgentState.Stationary] = 0.02f;
        T[(int)AgentState.Walking, (int)AgentState.Walking] = 1f - T[(int)AgentState.Walking, (int)AgentState.Stationary];


        

        // for (int n=0; n<N_npc; n++){

        //     //print(String.Format("{0:0.00}",npcs[n] ));
        //     print("pos "+string.Join(", ", agents[n].transform.position));
        //     //npcs[n].GetComponent<Rigidbody>().velocity = random_vel(-0.1f,0.1f);
            
        //     //print("vel "+string.Join(", ", npcs[n].GetComponent<Rigidbody>().velocity)); 
        // }
        //print(Util.describe(typeof(npcs[0]) ) );

    }


    enum Action {
        Walk,
        Run
    }

    Action? getAction() {
        var deadZone = 0.3f;
        if (Input.GetAxisRaw("Horizontal") > deadZone) {
            // print("Walking Action");
            return Action.Walk;
        }
        // else if(Input.GetKeyDown(KeyCode.Joystick1Button3)) {
        //     // print("Running Action");
        //     return Action.Run; 
        // }
        else if(Input.GetButton("Fire1")) {
            // print("Running Action");
            return Action.Run; 
        }
        // else if(Input.GetButtonDown("Y")) {
        //     // print("Running Action");
        //     return Action.Run; 
        // }
        // else if(Input.GetButton("Y")) {
        //     // print("Running Action");
        //     return Action.Run; 
        // }
        // else if(Input.GetKeyDown(KeyCode.Joystick1Button3)) {
        //     // print("Running Action");
        //     return Action.Run; 
        // }
        // else if(Input.GetKeyDown(KeyCode.JoystickButton3)) {
        //     // print("Running Action");
        //     return Action.Run; 
        // }
        // else if(Mathf.Abs(Input.GetAxis("joystick 1 button 3")) > deadZone) {
        //     // print("Running Action");
        //     return Action.Run; 
        // }
        return null;
    }
    
    Vector3 walkingVelocity = new Vector3(1f,0f,0f);
    Vector3 runningVelocity = new Vector3(1.8f,0f,0f);
    // Update is called once per frame
    void Update()
    {
        var action = getAction();
        processAction(players[0], action);
        foreach  (GameObject agent in agents) {
            var nextState = decideNextState(agent);
            agentStates[agent.GetInstanceID()] = nextState;
            agent.GetComponent<Rigidbody>().velocity = nextState == AgentState.Stationary ? Vector3.zero : walkingVelocity;
        }
    }

    void processAction(GameObject player, Action? action) {
        print("processing action: " + action.ToString());
        var state = agentStates[player.GetInstanceID()];

        if (action == null) {
            agentStates[player.GetInstanceID()] = AgentState.Stationary;
            if(state != AgentState.Stationary) {
                player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        else if (action == Action.Walk) {
            agentStates[player.GetInstanceID()] = AgentState.Walking;
            if(state != AgentState.Walking) {
                player.GetComponent<Rigidbody>().velocity = walkingVelocity;
            }
        }
        else if (action == Action.Run) {
            agentStates[player.GetInstanceID()] = AgentState.Running;
            if(state != AgentState.Running) {
                player.GetComponent<Rigidbody>().velocity = runningVelocity;
            }
        }
    }


    AgentState decideNextState(GameObject agent) {
        float v = Random.Range(0f,1f);
        //print("Random Number: " + v.ToString());
        var prevState = agentStates[agent.GetInstanceID()];
        if (v < T[(int) prevState, (int)AgentState.Stationary]) {
            //print("Going STATIONARY BABAAYYYY");
            return AgentState.Stationary;
        }
        else {
            //print("MOVING");
            return AgentState.Walking;
        }

        // if (state == AgentState.Stationary) {
        //     if (v < T[state][]) {
        //         agentStates[agent.GetInstanceID()] = AgentState.Moving;
        //         return walkingVelocity;
        //     }
        //     return 0f;
        // }
        // else {
        //    if (v < ) {
        //    }    
        // }

    }

    void FixedUpdate()
    {
        
    }

}
