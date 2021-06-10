using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FeatherCollectible : MonoBehaviour, ICollectible
{
    private float defaultY;
    private float direction;

    private void Awake()
    {
        defaultY = transform.position.y;
        direction = 1;
    }

    public void Update()
    {
        FloatingAnimation();
    }

    void FloatingAnimation()
    {
        // Floating Animation
        float maxYOffset = 0.3f;
        float yToIncrease = 0.4f;
        float angletoAdd = 100f;

        // Rotates
        this.transform.Rotate(Vector3.up, angletoAdd * Time.deltaTime);

        // Goes up and down
        if (transform.position.y > defaultY + maxYOffset) direction = -1;
        else if (transform.position.y < defaultY - maxYOffset) direction = 1;

        transform.position = new Vector3(transform.position.x,
            transform.position.y + yToIncrease * direction * Time.deltaTime, transform.position.z);
    }

    public void CollectedAction(HealthManager player)
    {
        player.FeatherCollected();
    }

    void OnTriggerEnter(Collider other)
    {
        // Checks if is player
        HealthManager playerHealthManagerTemp = other.gameObject.GetComponent<HealthManager>();
        if (playerHealthManagerTemp == null) return;

        // Collects
        CollectedAction(playerHealthManagerTemp);
        Destroy(this.gameObject);
    }
}