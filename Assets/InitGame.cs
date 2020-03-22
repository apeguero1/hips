using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGame : MonoBehaviour
{
    public GameObject Game;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       bool? hostDecision = IAmHost();
       if(hostDecision != null) {
           var game = Instantiate(Game);
           game.GetComponent<DeathRaceBehaviour>().IAmHost = hostDecision.Value;
           Destroy(this);
       }
    }

    bool? IAmHost() {
        return Input.GetKeyDown("h") ? true 
            : Input.GetKeyDown("c") ? false
            : (bool?) null;
        
    }

}
