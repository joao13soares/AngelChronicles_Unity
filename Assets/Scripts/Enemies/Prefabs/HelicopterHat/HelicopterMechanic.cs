using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterMechanic : MonoBehaviour
{
    GameObject player;
    [SerializeField] float flyingVelocity;
    [SerializeField] float maxFlyingTime;
    float currentFlyingTime = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        this.transform.localPosition += player.transform.lossyScale.y * 1.25f * player.transform.up;
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
