using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefabConsume :  IConsumable
{
    
    
    public void ConsumeAction( Transform playerTransform, GameObject prefabToInstantiate)
    {
        GameObject.Instantiate(prefabToInstantiate,playerTransform.position, playerTransform.rotation, playerTransform);
    }
}
