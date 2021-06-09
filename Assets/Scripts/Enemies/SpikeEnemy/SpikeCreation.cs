using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCreation : MonoBehaviour
{

    

    private void Awake()
    {
        this.enabled = false;
    }

    void OnCollisionEnter(Collision other)
    {

        if(other.gameObject.CompareTag("Wall"))
        {
            GameObject spikes = Instantiate(this.GetComponent<SpikeEnemy>().spikePrefab, this.transform.position,GameObject.Find("Player").transform.rotation,null);
            GameObject.Destroy(this.gameObject);
        }


        //if (other.gameObject.CompareTag("Wall"))
        //{
        //    this.transform.parent = null;
        //    this.transform.localScale = new Vector3(1.3f, 0.3f, 2f);

        //    this.transform.parent = other.transform;
        //    // this.transform.localScale = new Vector3(transform.lossyScale.x, transform.lossyScale.y, other.transform.lossyScale.z) ;

            
        //    this.GetComponent<Rigidbody>().isKinematic = true;
        //    this.tag = "Ground";


        //}
    }
}
