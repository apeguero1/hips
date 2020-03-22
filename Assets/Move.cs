using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class Move : MonoBehaviour
{
    Vector3 random_vel(float a,float b){
        return new Vector3(Random.Range(a,b),
            Random.Range(a,b),0);
    }

    GameObject makeFinishLine(){
        var finishLine = Instantiate(Block, new Vector3(+Global.X/2,
                                              -Global.Y/2*0f ,0), 
                                              Quaternion.identity);
        finishLine.AddComponent<Rigidbody>();                
        finishLine.GetComponent<Collider>().enabled = false;
        finishLine.GetComponent<Rigidbody>().useGravity = false;
        finishLine.transform.localScale = new Vector3 (1,Global.Y, 1);
        return finishLine;
    }
/*
     Dictionary<int,AgentState> makeCrossHairs(){
        Dictionary<int,GameObject> agentStates = new Dictionary<int,GameObject>();
        
        var finishLine = Instantiate(Block, new Vector3(+Global.X/2,
                                              -Global.Y/2*0f ,0), 
                                              Quaternion.identity);
        
        //finishLine.AddComponent<Rigidbody>();                
        finishLine.GetComponent<Collider>().enabled = false;
        //finishLine.GetComponent<Rigidbody>().useGravity = false;
        finishLine.transform.localScale = new Vector3 (Global.X,Global.Y, 1);
        return finishLine;
    }*/



    Vector3 walkingVelocity = new Vector3(2f,0f,0f);
    public GameObject Block;
    float var1;

    //List<GameObject> npcs = new List<GameObject>();
    //List<Rigidbody> rb_npcs = new List<Rigidbody>();
    List<GameObject> agents = new List<GameObject>();

    List<GameObject> players = new List<GameObject>();
    enum AgentState
    {
        Stationary,
        Moving
    }

    //var agentStates = new List<AgentState>();
    Dictionary<int,AgentState> agentStates = new Dictionary<int,AgentState>();

    //List<physics> npcs = new List<GameObject>();

    int N_agent = 10;
    int N_player = 2;
    
    

    // Start is called before the first frame update
    
    float[,] T = new float[2, 2];
    //int p1 = Random.Range(0,N_npc);
    List<int> playersI = new List<int>();
    int currentPlace = 1;
    void Start()
    {
        //print("AgentState.Stationary: " + (int) AgentState.Stationary);


        var finishLine = makeFinishLine();

        
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
                
                
                if (playersI.Contains(n) ){
                    players.Add(agent);
                    }
                else{
                    agents.Add(agent);
                    agentStates.Add(agent.GetInstanceID(),AgentState.Stationary);
                }



            }

        T[(int)AgentState.Stationary, (int)AgentState.Stationary] = 0.997f;
        T[(int)AgentState.Stationary, (int)AgentState.Moving] = 1f - T[(int)AgentState.Stationary, (int)AgentState.Stationary];
        T[(int)AgentState.Moving, (int)AgentState.Stationary] = 0.003f;
        T[(int)AgentState.Moving, (int)AgentState.Moving] = 1f - T[(int)AgentState.Moving, (int)AgentState.Stationary];


        

        // for (int n=0; n<N_npc; n++){

        //     //print(String.Format("{0:0.00}",npcs[n] ));
        //     print("pos "+string.Join(", ", agents[n].transform.position));
        //     //npcs[n].GetComponent<Rigidbody>().velocity = random_vel(-0.1f,0.1f);
            
        //     //print("vel "+string.Join(", ", npcs[n].GetComponent<Rigidbody>().velocity)); 
        // }
        //print(Util.describe(typeof(npcs[0]) ) );

    }


    
    // Update is called once per frame
    List<int> finishedRace = new List<int>();

    void Update()
    {


        foreach  (GameObject agent in agents) {
            var nextState = decideNextState(agent);
            agentStates[agent.GetInstanceID()] = nextState;
            agent.GetComponent<Rigidbody>().velocity = nextState == AgentState.Stationary ? Vector3.zero : walkingVelocity;
       

       }

        for (int n=0; n<N_agent - N_player; n++){
            var agent = agents[n];
            var id = agent.GetInstanceID();
            int winnerCount = 0;
            if ( agent.transform.position[0] >= Global.X/2 &  !finishedRace.Contains(id) ){
                print("Position: "+currentPlace.ToString()+" "+id.ToString()+" "+n.ToString() );
                finishedRace.Add(agent.GetInstanceID());
                winnerCount = winnerCount + 1;
                }
            if (winnerCount > 0){
                currentPlace = currentPlace + 1;
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
            return AgentState.Moving;
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