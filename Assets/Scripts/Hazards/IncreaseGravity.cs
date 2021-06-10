using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseGravity : MonoBehaviour
{
    private void OnCollisionStay(Collision other)
    {
        // Increase Player Gravity
        if(other.collider.gameObject.CompareTag("Player"))
            other.collider.gameObject.GetComponent<Rigidbody>().velocity+= Vector3.down * 10;
    }
}
