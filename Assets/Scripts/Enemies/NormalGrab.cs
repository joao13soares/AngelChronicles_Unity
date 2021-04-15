using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGrab : IGrabbable
{

    public void GrabAction(GameObject objectToGrab, float scalingFactor, Transform handTransform, Quaternion handRotation, Vector3 handPosition, out Vector3 defaultScale)
    {
        Rigidbody rb = objectToGrab.GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.useGravity = false;
        rb.velocity = Vector3.zero; // cancel all existing forces on this rigidbody

        defaultScale = objectToGrab.transform.localScale; // save defaultScale of this grabbedObj
        objectToGrab.transform.localScale = handTransform.transform.localScale.normalized * scalingFactor; // scale grabbedObj to fit the hand object
        objectToGrab.transform.parent = handTransform;
        objectToGrab.transform.rotation = handRotation;
        objectToGrab.transform.position = handPosition;
        objectToGrab.transform.Translate((objectToGrab.transform.lossyScale.z + handTransform.GetChild(0).lossyScale.z) * 0.75f * handTransform.forward, Space.World); // position this grabbedObj in front of the hand object
        
    }
}
