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
        float maxYOffset = 0.3f;
        float yToIncrease = 0.4f;
        float angletoAdd = 100f;

        this.transform.Rotate(Vector3.up, angletoAdd * Time.deltaTime);

        if (transform.position.y > defaultY + maxYOffset) direction = -1;
        else if (transform.position.y < defaultY - maxYOffset) direction = 1;

        transform.position = new Vector3(transform.position.x,transform.position.y + yToIncrease * direction * Time.deltaTime,transform.position.z);

    }

    public void CollectedAction(HealthManager player)
    {
        player.FeatherCollected();
    }

    void OnCollisionEnter(Collision other)
    {
        HealthManager playerHealthManagerTemp = other.gameObject.GetComponent<HealthManager>();

        if (playerHealthManagerTemp == null) return;

        CollectedAction(playerHealthManagerTemp);

        Destroy(this.gameObject);

    }
}
