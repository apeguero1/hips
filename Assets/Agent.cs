using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{

    public enum State
    {
        Stationary,
        Walking,
        Running
    }

    public enum Action {
        Walk,
        Run
    }

    public bool isNpc = true;
    public int Id;
    public int playerNum;
    public State state = State.Stationary;
    public Vector3 walkingVelocity = new Vector3(1f,0f,0f);
    public Vector3 runningVelocity = new Vector3(1.8f,0f,0f);
    // Start is called before the first frame update

    float[,] T = new float[2,2];
    void Start()
    {
        T[(int)Agent.State.Stationary, (int)Agent.State.Stationary] = 0.97f;
        T[(int)Agent.State.Stationary, (int)Agent.State.Walking] = 1f - T[(int)Agent.State.Stationary, (int)Agent.State.Stationary];
        T[(int)Agent.State.Walking, (int)Agent.State.Stationary] = 0.02f;
        T[(int)Agent.State.Walking, (int)Agent.State.Walking] = 1f - T[(int)Agent.State.Walking, (int)Agent.State.Stationary];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void performAction(Action? action) {
        // print("processing action: " + action.ToString());
        if (action == null) {
            if(state != Agent.State.Stationary) {
                state = Agent.State.Stationary;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        else if (action == Action.Walk) {
            if(state != Agent.State.Walking) {
                state  = Agent.State.Walking;
                GetComponent<Rigidbody>().velocity = walkingVelocity;
            }
        }
        else if (action == Action.Run) {
            if(state != Agent.State.Running) {
                state  = Agent.State.Running;
                GetComponent<Rigidbody>().velocity = runningVelocity;
            }
        }
    }

    public Action? getAction() {
        if(!isNpc) {
           return getPlayerAction();
        }
        else {
           return getNpcAction(); 
        }
    }

    Action? getPlayerAction() {
        var deadZone = 0.3f;
        if (Input.GetAxisRaw("Horizontal") > deadZone) {
            // print("Walking Action");
            return Action.Walk;
        }
        else if(Input.GetButton("Fire1")) {
            // print("Running Action");
            return Action.Run; 
        }
        // else if(Input.GetKeyDown(KeyCode.Joystick1Button3)) {
        //     // print("Running Action");
        //     return Action.Run; 
        // }
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
    
    Action? getNpcAction() {
        float v = Random.Range(0f,1f);
        if (v < T[(int) state, (int)Agent.State.Stationary]) {
            return Action.Walk;
        }
        else {
            return null;
        }
    }

    //  float _yRotation = Input.GetAxisRaw("Mouse X");
    //         float _xRotation = Input.GetAxisRaw("Mouse Y");
    //         Vector3 _rotation = new Vector3(0f, _yR


}
