using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabbable
{
    void GrabAction(GameObject objectToGrab, float scalingFactor, Transform handTransform, Quaternion handRotation, Vector3 handPosition, out Vector3 defaultScale);
}
