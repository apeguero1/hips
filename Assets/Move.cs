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


    public GameObject Block;
    float var1;

    //List<GameObject> npcs = new List<GameObject>();
    //List<Rigidbody> rb_npcs = new List<Rigidbody>();
    List<GameObject> agents = new List<GameObject>();
    enum AgentState
    {
        Stationary,
        Moving
    }

    //var agentStates = new List<AgentState>();
    Dictionary<int,AgentState> agentStates = new Dictionary<int,AgentState>();

    //List<physics> npcs = new List<GameObject>();

    int N_npc = 10;
    // Start is called before the first frame update
    
    float[,] T = new float[2, 2];
    void Start()
    {
        print("AgentState.Stationary: " + (int) AgentState.Stationary);
        for (int n=0; n<N_npc; n++)
            {
                Random.Range(-10.0f, 10.0f);
                /*npcs.Add(Instantiate(Block2, new Vector3(Random.Range(-Global.X/2,Global.X/2),
                                              Random.Range(-Global.Y/2,Global.Y/2),0), 
                                              Quaternion.identity)
                */

                
                var agent = Instantiate(Block, new Vector3(-Global.X/2,
                                              -Global.Y/2 + n*Global.Y/N_npc   ,0), 
                                              Quaternion.identity);

                agent.AddComponent<Rigidbody>();
                
                agent.GetComponent<Collider>().enabled = false;
                agent.GetComponent<Rigidbody>().useGravity = false;

                agent.transform.localScale = new Vector3 (1, 1, 1)*0.5f;
                agents.Add(agent);
                agentStates.Add(agent.GetInstanceID(),AgentState.Stationary);

            }

        T[(int)AgentState.Stationary, (int)AgentState.Stationary] = 0.97f;
        T[(int)AgentState.Stationary, (int)AgentState.Moving] = 1f - T[(int)AgentState.Stationary, (int)AgentState.Stationary];
        T[(int)AgentState.Moving, (int)AgentState.Stationary] = 0.02f;
        T[(int)AgentState.Moving, (int)AgentState.Moving] = 1f - T[(int)AgentState.Moving, (int)AgentState.Stationary];

        // for (int n=0; n<N_npc; n++){

        //     //print(String.Format("{0:0.00}",npcs[n] ));
        //     print("pos "+string.Join(", ", agents[n].transform.position));
        //     //npcs[n].GetComponent<Rigidbody>().velocity = random_vel(-0.1f,0.1f);
            
        //     //print("vel "+string.Join(", ", npcs[n].GetComponent<Rigidbody>().velocity)); 
        // }
        //print(Util.describe(typeof(npcs[0]) ) );

    }


    Vector3 walkingVelocity = new Vector3(1f,0f,0f);
    // Update is called once per frame
    void Update()
    {
       foreach  (GameObject agent in agents) {
            var nextState = decideNextState(agent);
            agentStates[agent.GetInstanceID()] = nextState;
            agent.GetComponent<Rigidbody>().velocity = nextState == AgentState.Stationary ? Vector3.zero : walkingVelocity;
       }

    }


    AgentState decideNextState(GameObject agent) {
        float v = Random.Range(0f,1f);
        print("Random Number: " + v.ToString());
        var prevState = agentStates[agent.GetInstanceID()];
        if (v < T[(int) prevState, (int)AgentState.Stationary]) {
            print("Going STATIONARY BABAAYYYY");
            return AgentState.Stationary;
        }
        else {
            print("MOVING");
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
