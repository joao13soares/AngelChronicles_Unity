using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpMechanic : MonoBehaviour
{
    GameObject player;
    private PlayerMovement playerMov;
    [SerializeField] float bounciness;
    [SerializeField] float maxBounceTime;
    float currentBounceTime = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMov = player.GetComponent<PlayerMovement>();
        //
        // this.transform.localPosition -= player.transform.lossyScale.y * 1f * player.transform.up;
         this.transform.localScale /=   player.transform.localScale.x ;
         
         playerMov.PlayerJump();

        // player.GetComponent<Rigidbody>().AddForce(bounciness * this.transform.up, ForceMode.Impulse);
        
    }

    void Update()
    {

        playerMov.isGrounded = true;
        
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
