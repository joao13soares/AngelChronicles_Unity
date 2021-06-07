using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementForce;
    [SerializeField] float jumpForce;
    Rigidbody playerRb;
    [SerializeField] public bool isGrounded, isRoofed;
    public bool isMagnetized;
    public bool isDoubleJumping;
    public bool isOnHeli;
    private HealthManager playerHealthManager;

    [SerializeField] Camera camera;

    bool jumpQueued;
    public bool jumping;

    void Awake()
    {
        playerRb = this.GetComponent<Rigidbody>();
        playerHealthManager = this.GetComponent<HealthManager>();
        playerHealthManager.playerRespawned += ResetGravity;
    }

    void Update()
    {
        JumpInput();
    }
    void FixedUpdate()
    {
        MovementInput();
        JumpAction();

        if (isMagnetized)
        {
            Magnetize();
        }
        else
        {
            Demagnetize();
        }
    }

    void Magnetize()
    {
        playerRb.useGravity = false;
        playerRb.AddForce(-Physics.gravity, ForceMode.Acceleration);

        if (!isRoofed && !jumping && !isOnHeli && !isDoubleJumping)
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(Vector3.right * 180), 4 * Time.deltaTime);
        }
    }

    void Demagnetize()
    {
        playerRb.useGravity = true;

        if (!isGrounded && !jumping && !isOnHeli && !isDoubleJumping)
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(Vector3.up * 180), 4 * Time.deltaTime);
        }
    }

    void ResetGravity()
    {
        playerRb.useGravity = true;
        jumping = false;
        isMagnetized = false;
    }

    void MovementInput()
    {
        Vector3 horMov = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (horMov.magnitude > 1) horMov.Normalize();
        horMov *= movementForce;

        float cameraAngleWithWorld = camera.transform.eulerAngles.y;

        // movement
        playerRb.velocity = Quaternion.AngleAxis(cameraAngleWithWorld, Vector3.up) * (playerRb.velocity.y * Vector3.up + horMov);

        if (horMov.magnitude == 0) return;

        // angle to face towards
        float angleToTurn = Mathf.Atan2(horMov.x, horMov.z) * Mathf.Rad2Deg + cameraAngleWithWorld;
        float smoothTurnVel = 2f;
        float smoothAngle = Mathf.SmoothDampAngle(this.transform.eulerAngles.y, angleToTurn, ref smoothTurnVel, 0.05f);
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, smoothAngle, this.transform.eulerAngles.z);
    }

    void JumpInput()
    {
        bool jumpKeyPressed = Input.GetKey(KeyCode.Space);

        if (jumpKeyPressed &&
            (isGrounded || isRoofed) &&
            !jumpQueued)
            jumpQueued = true;
    }

    void JumpAction()
    {
        if (!jumpQueued) return;

        Jump(jumpForce);

        jumpQueued = false;
        jumping = true;
        isGrounded = false;
        isRoofed = false;
    }

    public void Jump(float force)
    {
        playerRb.velocity = Vector3.zero;
        playerRb.AddForce(force * this.transform.up, ForceMode.Impulse);
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") && !isMagnetized)
        {
            isGrounded = true;
            jumping = false;
            if (!isDoubleJumping) playerRb.velocity = Vector3.zero;
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
        }

        if (other.gameObject.CompareTag("Roof") && isMagnetized)
        {
            isRoofed = true;
            jumping = false;
            if (!isDoubleJumping) playerRb.velocity = Vector3.zero;
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 180);
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }

        if (other.gameObject.CompareTag("Roof"))
        {
            isRoofed = false;
        }
    }
    
    
    
   
}
