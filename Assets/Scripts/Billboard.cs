using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Billboard : MonoBehaviour
{
    private Camera cam;

    //This script is used for making quad sprites to always face the camera 
    void Awake()
    {
        cam = Camera.main;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.transform.forward);
    }
}
