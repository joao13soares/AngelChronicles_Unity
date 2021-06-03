using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGrab : IGrabbable
{

    public virtual void GrabAction(GameObject objectToGrab, float scalingFactor, Transform grabbablePlaceHolderTransform, Quaternion handRotation, Vector3 handPosition, out Vector3 defaultScale)
    {
        

        objectToGrab.GetComponent<Collider>().enabled = false;
        objectToGrab.GetComponent<Enemy>().canBeGrabbed = false;
        objectToGrab.GetComponent<EnemyMovement>().enabled = false;

        
        Rigidbody rb = objectToGrab.GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.useGravity = false;
        rb.velocity = Vector3.zero; // cancel all existing forces on this rigidbody

        defaultScale = objectToGrab.transform.localScale; // save defaultScale of this grabbedObj
        objectToGrab.transform.localScale = objectToGrab.transform.localScale.normalized * scalingFactor; // scale grabbedObj to fit the hand object
        objectToGrab.transform.parent = grabbablePlaceHolderTransform;
        objectToGrab.transform.rotation = grabbablePlaceHolderTransform.rotation;
        objectToGrab.transform.localPosition = Vector3.zero;
        //objectToGrab.transform.Translate((objectToGrab.transform.lossyScale.z + handTransform.GetChild(0).lossyScale.z) * 0.75f * handTransform.forward, Space.World); // position this grabbedObj in front of the hand object
        
    }
}
