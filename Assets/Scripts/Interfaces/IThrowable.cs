using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThrowable 
{
    void ThrowAction(GameObject objectToThrow,  Vector3 velocity);
}
