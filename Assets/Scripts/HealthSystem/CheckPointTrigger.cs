using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    [SerializeField] private GameObject checkPointSign;
    
    [SerializeField]
    GameObject CheckPoint;

    [SerializeField]
    HealthManager healthManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            healthManager.lastCheckPointPosition = CheckPoint.transform.position;
            checkPointSign.GetComponent<Animation>().Play();
            
            
            Destroy(this.gameObject);
        }
    }
}
