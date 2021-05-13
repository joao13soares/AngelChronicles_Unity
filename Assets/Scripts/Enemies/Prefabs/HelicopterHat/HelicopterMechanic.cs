using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterMechanic : MonoBehaviour
{
    GameObject player;
    [SerializeField] float flyingVelocity;
    [SerializeField] float maxFlyingTime;
    float currentFlyingTime = 0;


    private Vector3 HeliHatOffset = Vector3.up + new Vector3(0.1f, 0.5f, -0.2f);
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        this.transform.localScale /=   player.transform.lossyScale.x ;
        this.transform.localPosition = HeliHatOffset/player.transform.lossyScale.x;
        player.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePositionY;
    }

    void Update()
    {
        if (currentFlyingTime >= maxFlyingTime)
        {
            GameObject.DestroyImmediate(this.gameObject);
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            player.transform.Translate(new Vector3(0, flyingVelocity, 0) * Time.deltaTime);
            currentFlyingTime += Time.deltaTime;

            this.transform.Rotate(new Vector3(0, flyingVelocity, 0));
        }
    }

   
}
