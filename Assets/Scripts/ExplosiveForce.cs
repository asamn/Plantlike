using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveForce : MonoBehaviour
{
    Rigidbody rb;
    public float thrust = 50f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.up * thrust);
    }

}
