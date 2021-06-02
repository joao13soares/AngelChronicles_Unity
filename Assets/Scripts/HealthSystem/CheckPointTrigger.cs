using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject CheckPoint;

    [SerializeField]
    HealthManager healthManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            healthManager.lastCheckPointPosition = CheckPoint.transform.position;
        }
    }
}
