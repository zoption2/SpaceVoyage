using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShadowOnSpaceObjects : MonoBehaviour
{

    
   void Start()
    {
        
    }


    void Update()
    {
        //Quaternion rotation = Quaternion.FromToRotation(transform.position, -transform.up);
        ////Quaternion rotation = Quaternion.LookRotation(transform.up);
        //transform.rotation = rotation * transform.rotation;
        //Debug.Log("Rot" + transform.up);

        transform.position = transform.parent.position;

    }
}
