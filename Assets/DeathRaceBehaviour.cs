using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class DeathRaceBehaviour : MonoBehaviour
{
    public GameObject AgentPrefab;
    public GameObject FinishLine;
    public bool IAmHost;

    List<Agent> agents = new List<Agent>();

    int N_agent = 10;
    int N_player = 2;
    
    
    // Start is called before the first frame update
    GameObject makeFinishLine(){
        var finishLine = Instantiate(FinishLine, new Vector3(+Global.X/2,
                                              -Global.Y/2*0f ,0), 
                                              Quaternion.identity);
        finishLine.transform.localScale = new Vector3 (1,Global.Y, 1);
        return finishLine;
    }

    void Start() {
        if(IAmHost) {
            hostStart();
        }
        else {
            // clientWaitForFirstState();
        }
    }

    void hostStart() {
        var finishLine = makeFinishLine();

        List<int> playersI = new List<int>();
        for (int n=0; n<N_player; n++) {
            var i = Random.Range(0,N_agent);
            while (playersI.Contains(i)) {
                i = Random.Range(0,N_agent);
            }
            playersI.Add(i);
        }
         
        for (int n=0; n<N_agent; n++) {
            var agentObj = Instantiate(AgentPrefab, new Vector3(-Global.X/2,
                                            -Global.Y/2 + n*Global.Y/N_agent,0),
                                            Quaternion.identity);
            var agent = agentObj.GetComponent<Agent>();
            agent.isNpc = playersI.Contains(n) ? false : true;
            agent.Id = n;
            agents.Add(agent);
        }
    }

    List<Agent> finishedRace = new List<Agent>();
    int currentPlace = 1;
    int winnerCount = 0;
    void Update() {
        foreach  (Agent agent in agents) {
            var action = agent.getAction();
            agent.performAction(action);
            if (finished(agent)) {
                print("Position: "+currentPlace.ToString()+" "+agent.Id.ToString());
                finishedRace.Add(agent); 
                currentPlace += 1;
            }
            // agent.GetComponent<Rigidbody>().velocity = nextState == Agent.State.Stationary ? Vector3.zero : walkingVelocity;
        }
    }

    bool finished(Agent agent) {
        return agent.transform.position[0] >= Global.X/2 && !finishedRace.Contains(agent);
    }

    // // ----------- host code ----------- 
    // // Job 1: Update function as is 
    // // Job 2
    // void listenForClientAction() {
    //    (Action? action, int playerId) = receiveAction();
    //    processAction(players[playerId], action);
    // }

    // // ----------- client code -----------
    // // Job 2
    // void listenForInputFromUser() {
    //     var action = getAction();
    //     sendActionToHost(action);
    // }

    // // Job 1
    // void listenForStateChangeFromHost() {
    //     List<(int agentId, Vector3,Vector3)> receivedState = receiveState();
    //     clientReceiveState(receivedState);
    // }

    // void clientReceiveState(List<(int, Vector3, Vector3)> hostState) {
    //    foreach ((int agentId, Vector3 pos, Vector3 vel) agentState in hostState) {
    //        updateState(agentState.agentId, agentState.pos, agentState.vel);
    //    }
    // }

    // void updateState(int agentId, Vector3 pos, Vector3 vel) {
    //     var agent = agents[agentId] ;
    //     agent.transform.position = pos;
    //     agent.GetComponent<Rigidbody>().velocity = vel;
    // }
}