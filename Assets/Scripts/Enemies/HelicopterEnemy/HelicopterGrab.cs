using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterGrab : NormalGrab
{
    public override void GrabAction(GameObject objectToGrab, float scalingFactor, Transform handTransform, Quaternion handRotation,
        Vector3 handPosition, out Vector3 defaultScale)
    {

        defaultScale = Vector3.one; // out needs to be assigned so this is here to make compiling possible

       
        if (objectToGrab.GetComponent<ShockingTimer>().doesDamageToPlayer)
        {
            objectToGrab.GetComponent<PlayerContact>().DamagePlayer(GameObject.FindGameObjectWithTag("Player").GetComponent<HealthManager>());

        }

        
        else 
            base.GrabAction(objectToGrab, scalingFactor, handTransform, handRotation, handPosition, out defaultScale);
        
        
    }
}
