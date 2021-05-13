using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpMechanic : MonoBehaviour
{
    GameObject player;
    [SerializeField] float bounciness;
    [SerializeField] float maxBounceTime;
    float currentBounceTime = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //
        // this.transform.localPosition -= player.transform.lossyScale.y * 1f * player.transform.up;
         this.transform.localScale /=   player.transform.localScale.x ;

        player.GetComponent<Rigidbody>().AddForce(bounciness * this.transform.up, ForceMode.Impulse);
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) || currentBounceTime >= maxBounceTime)
        {
            GameObject.DestroyImmediate(this.gameObject);
        }
        else
        {
            currentBounceTime += Time.deltaTime;
        }
    }
}
