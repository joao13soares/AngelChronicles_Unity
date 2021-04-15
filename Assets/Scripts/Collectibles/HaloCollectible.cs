using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaloCollectible : MonoBehaviour, ICollectible
{
    
    public void CollectedAction(HealthManager player)
    {
        player.HaloCollected();
    }

    void OnCollisionEnter(Collision other)
    {
        HealthManager playerHealthManagerTemp = other.gameObject.GetComponent<HealthManager>();

        if (playerHealthManagerTemp == null) return;

        CollectedAction(playerHealthManagerTemp);

        Destroy(this.gameObject);

    }


}
