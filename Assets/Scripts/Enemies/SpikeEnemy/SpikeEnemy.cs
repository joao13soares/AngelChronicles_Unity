using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeEnemy : Enemy
{
   
    void Awake()
    {
        canBeGrabbed = true;
        grabAction = new SpikeGrab();
        throwAction = new SpikeThrow();
    }
}
