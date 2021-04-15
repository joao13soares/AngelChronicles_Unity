using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterEnemy : Enemy
{
    // Start is called before the first frame update
    void Awake()
    {
        canBeGrabbed = true;
        grabAction = new NormalGrab();
        consumableAction = new SpawnPrefabConsume();
        throwAction = new NormalThrow();
    }

    
}
