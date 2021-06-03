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

    [SerializeField] private GameObject poofSmokePrefab;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        player.GetComponent<PlayerMovement>().isDoubleJumping = true;
        player.GetComponent<PlayerAnim>().jumpConsumableAnimator = this.GetComponent<Animator>();

        //this.transform.position = player.transform.position - this.GetComponent<MeshRenderer>().bounds.extents.y * player.transform.up;

        if (!player.GetComponent<PlayerMovement>().jumping)
        {
            player.GetComponent<PlayerMovement>().PlayerJump(bounciness);
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) || currentBounceTime >= maxBounceTime)
        {
            player.GetComponent<PlayerMovement>().PlayerJump(bounciness);

            player.GetComponent<PlayerMovement>().isDoubleJumping = false;
            
            Instantiate(poofSmokePrefab, player.transform.GetComponentInChildren<SkinnedMeshRenderer>().bounds.center, Quaternion.identity, null);

            GameObject.DestroyImmediate(this.gameObject);
        }
        else
        {
            currentBounceTime += Time.deltaTime;
        }
    }
}