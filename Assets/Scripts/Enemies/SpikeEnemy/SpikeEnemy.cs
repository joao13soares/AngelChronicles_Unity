using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeEnemy : Enemy
{
   
    void Awake()
    {
        canBeGrabbed = true;
        grabAction = new NormalGrab();
        throwAction = new SpikeThrow();
    }
}
