using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FirstScript : MonoBehaviour
{

    
    public static float pi =(float) Math.PI;
    public float speedx = 0.5f;
    public float v = (float)(1.0*2.0)*pi;
    public float v2 = (float)(0.1*2.0)*pi;
    //public float v2 =  (float)1.0*2*(float)pi ;
    //v2 = v2/10.0;
    // Start is called before the first frame update
    
    void Start()
    {
        
        print("Start!");
        print(String.Format("{0:0.00}",v));
        print(String.Format("{0:0.00}",v2));
        //print(String.Format("{0:0.00}",Global.width));
        //print("an object");
    }

    // Update is called once per frame
    void Update()
    {        
    float t = Time.fixedTime;
    float xpos = (float)Math.Cos(t*v2);
    float ypos = (float)Math.Sin(t*v2);
    
    //float zpos = (float)Math.Cos(t*v/2);
    transform.position = new Vector3(xpos,ypos,0);
    //transform.Translate(speedx,0.1f,0.0f);
    //print(Time.fixedTime);   
    }
}
