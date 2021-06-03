using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementForce;
    [SerializeField] public float jumpForce;
    Rigidbody playerRb;
    [SerializeField] public bool isGrounded, isRoofed;
    public bool isDoubleJuping;
    public bool isMagnetized;

    [SerializeField] Camera camera;

    public bool jumpQueued;
    public bool jumping;
    
    
    private float gravityDirection;
    public float gravityForce;

    [SerializeField] private float rotationSpeed;
    
    
    
    void Awake()
    {
        playerRb = this.GetComponent<Rigidbody>();
        gravityDirection = 1;
    }

    void Update()
    {
        JumpInput();
    }
    void FixedUpdate()
    {
        MovementInput();
        JumpAction();

        playerRb.AddForce(Physics.gravity * gravityDirection, ForceMode.Acceleration);

        
    }


    public void ChangeGravityDirection()
    {
        switch (gravityDirection)
        {
            case 1:
                StartCoroutine(RotateToCeiling());
                break;
            case -1:
                break;
                
        }

        gravityDirection *= -1;
    }
    // void Magnetize()
    // {
    //     playerRb.AddForce(-Physics.gravity, ForceMode.Acceleration);
    //
    //     if(!jumping) this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(Vector3.right * 180), Time.deltaTime);
    // }

    void Demagnetize()
    {
        if (!jumping)this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(Vector3.up * 180), Time.deltaTime );
    }


    IEnumerator RotateToCeiling()
    {
        // float tolerance = 1f;
        //
        //
        // while (Mathf.Abs(transform.eulerAngles.z ) <=  180f - tolerance )
        // {
        //     
        //     
        //     Debug.Log(transform.eulerAngles.z);
        //     this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180), Time.deltaTime * rotationSpeed);
        //     yield return null;
        //     
        //     
        //
        // }
        //
        
        
        while(transform.rotation.z < 180)
        {
           // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(), rotationSpeed * Time.time);
           //  yield return null;

            transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles,
                new Vector3(transform.rotation.x, transform.rotation.y, 180), Time.deltaTime);
            yield return null;

        }

        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
        yield return null;

        

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


        PlayerJump();
        
        jumpQueued = false;
        jumping = true;
        isGrounded = false;
        isRoofed = false;
    }
    
    public void PlayerJump() =>  playerRb.AddForce(jumpForce * this.transform.up, ForceMode.Impulse);


    void OnCollisionStay(Collision other)
    {
        
        
        if (other.gameObject.CompareTag("Ground") && !isMagnetized)
        {
            isGrounded = true;
            jumping = false;
            if (!isDoubleJuping) playerRb.velocity = Vector3.zero;
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
        }

        if (other.gameObject.CompareTag("Roof") && isMagnetized)
        {
            isRoofed = true;
            jumping = false;
            if (!isDoubleJuping) playerRb.velocity = Vector3.zero;
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
