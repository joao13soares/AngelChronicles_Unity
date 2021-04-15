using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    
    [SerializeField] private float movementForce;
    private Rigidbody playerRb;
    private bool isGrounded;

    public bool canDoubleJump;
    
    
    // Movement variables storage for responsive rb movement
    private float movementX, movementZ;
    private bool jumpQueued;
    
    // Start is called before the first frame update
    void Awake()
    {
        playerRb = this.GetComponent<Rigidbody>();
        canDoubleJump = false;
    }

    // Update is called once per frame

    void Update()
    {
        JumpInput();
    }
    void FixedUpdate()
    {
        MovementInput();
        JumpAction();
    }


    private void MovementInput()
    {
        float x = Input.GetAxis("Horizontal") * movementForce;
        float z = Input.GetAxis("Vertical") * movementForce;

        float cameraAngleWithWorld = mainCamera.transform.eulerAngles.y;

        // movement
        playerRb.velocity = Quaternion.AngleAxis(cameraAngleWithWorld, Vector3.up) * new Vector3(x, playerRb.velocity.y, z);

        if (x == 0f && z == 0f) return;

        // angle to face towards
        float angleToTurn = Mathf.Atan2(x, z) * Mathf.Rad2Deg + cameraAngleWithWorld;
        float smoothTurnVel = 2f;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angleToTurn, ref smoothTurnVel, 0.05f);
        this.transform.eulerAngles = new Vector3(0f, smoothAngle, 0f);
        
    }
    
    private void JumpInput()
    {
        bool jumpKeyPressed = Input.GetKeyDown(KeyCode.Space);




        if (jumpKeyPressed &&
            isGrounded &&
            !jumpQueued) jumpQueued = true;

        else if (jumpKeyPressed && canDoubleJump)
        {
            jumpQueued = true;
            canDoubleJump = false;
        } 
    }

    private void JumpAction()
    {
        if (!jumpQueued) return;
        
        playerRb.AddForce(Vector3.up * movementForce, ForceMode.Impulse);

        jumpQueued = false;
        isGrounded = false;
        

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            //canDoubleJump = true;
        }
            
    }
}
