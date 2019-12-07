using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisiondetection : MonoBehaviour
{

    public bool isbuttonpressed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Button Collision Detected");

        isbuttonpressed = true;

    }
}
