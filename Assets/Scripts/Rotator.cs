using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Transform pointRotate;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0f)
        {
            transform.RotateAround(pointRotate.position, pointRotate.forward, 3f * Input.GetAxis("Horizontal") );
            
        }
       
    }
}
