using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HelicopterMechanic : MonoBehaviour
{
    
    
    
    GameObject player;
    [SerializeField] float flyingVelocity;
    [SerializeField] float maxFlyingTime;
    float currentFlyingTime = 0;


    [SerializeField] private GameObject poofSmokePrefab;

    void Start()
    {
        player = GameObject.Find("Player");

        //this.transform.localPosition += player.transform.lossyScale.y * 1.25f * player.transform.up;
        player.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePositionY;

        player.GetComponent<PlayerMovement>().isOnHeli = true;
    }

    void Update()
    {
        if (currentFlyingTime >= maxFlyingTime)
        {
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

            player.GetComponent<PlayerMovement>().isOnHeli = false;
            
            Instantiate(poofSmokePrefab, player.transform.GetComponentInChildren<SkinnedMeshRenderer>().bounds.center, Quaternion.identity, null);
            
            GameObject.DestroyImmediate(this.gameObject);
        }
        else
        {
            player.transform.Translate(player.transform.up * flyingVelocity * Time.deltaTime);
            currentFlyingTime += Time.deltaTime;

            //this.transform.Rotate(new Vector3(0, flyingVelocity, 0));
        }
    }
}