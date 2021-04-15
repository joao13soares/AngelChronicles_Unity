using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCreation : MonoBehaviour
{
    private void Awake()
    {
        this.enabled = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            this.transform.parent = other.transform;
            this.GetComponent<Rigidbody>().isKinematic = true;
            
        }
    }
}
