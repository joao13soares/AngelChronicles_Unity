using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpMechanic : MonoBehaviour
{
    GameObject player;
    [SerializeField] float bounciness;
    [SerializeField] float maxBounceTime;
    float currentBounceTime = 0;

    float defaultPlayerJumpForce;

    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().isDoubleJuping = true;
        this.transform.position = player.transform.position - this.GetComponent<MeshRenderer>().bounds.extents.y * player.transform.up;
        //player.GetComponent<Rigidbody>().AddForce(bounciness * this.transform.up, ForceMode.Impulse);
        defaultPlayerJumpForce = player.GetComponent<PlayerMovement>().jumpForce;
        player.GetComponent<PlayerMovement>().jumpForce = bounciness;
        if (!player.GetComponent<PlayerMovement>().jumping) player.GetComponent<PlayerMovement>().jumpQueued = true;
    }

    void Update()
    {
        player.GetComponent<PlayerMovement>().isGrounded = true;
        player.GetComponent<PlayerMovement>().isRoofed = true;

        if (Input.GetKey(KeyCode.Space) || currentBounceTime >= maxBounceTime)
        {
            player.GetComponent<PlayerMovement>().jumpForce = defaultPlayerJumpForce;
            player.GetComponent<PlayerMovement>().jumpQueued = true;
            player.GetComponent<PlayerMovement>().isDoubleJuping = false;
            GameObject.DestroyImmediate(this.gameObject);
        }
        else
        {
            currentBounceTime += Time.deltaTime;
        }
    }
}