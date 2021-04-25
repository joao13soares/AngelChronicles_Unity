using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalThrow : IThrowable
{
    public virtual void  ThrowAction(GameObject objectToThrow, Vector3 velocity)
    {
       
        objectToThrow.transform.parent = null;
        objectToThrow.transform.rotation = Quaternion.identity;

         Rigidbody rb = objectToThrow.GetComponent<Rigidbody>();
         Collider collider  = objectToThrow.GetComponent<Collider>();

        
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = true;
        rb.velocity = velocity; // thrown this grabbedObj

        collider.enabled = true;

        //StartCoroutine(Rescale(0.05f, defaultScale)); // rescale after some instant

    }
}
