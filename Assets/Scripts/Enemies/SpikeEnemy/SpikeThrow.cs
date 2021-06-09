using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeThrow : NormalThrow
{
    public override void ThrowAction(GameObject objectToThrow,Vector3 velocity)
    {
        base.ThrowAction(objectToThrow,velocity);
        objectToThrow.AddComponent<SpikeCreation>();
    }
}
