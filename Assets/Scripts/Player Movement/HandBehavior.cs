using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBehavior : MonoBehaviour
{
    public enum HandStates
    {
        OnBody,
        Thrown,
        Returning
    }

    public HandStates handState;
    [SerializeField] float speed;
    [SerializeField] float maxDist;
    [SerializeField] Transform handHolder;
    
    
     Vector3 thrownInitialPos;
     public Enemy grabbedObj;
     [SerializeField] private Transform grabbablePlaceHolder;

    [SerializeField] public float maxThrowingVelocity;
    public float currentThrowingVelocity;

    [SerializeField] float maxChargePitch;
    float currentChargePitch;
    [SerializeField] float pitchIncreaseRate;
    [SerializeField] ChargeLineRenderer chargeLineRenderer;

    int valuesChangingDir = 1;
    
    
    // SMOKE EFFECT

    [SerializeField] private GameObject poofSmokePrefab;

    void Update()
    {
        if(grabbedObj != null)
        grabbedObj.GrabbedBehavior(grabbablePlaceHolder.position.y);
        
        switch (handState)
        {
            case HandStates.OnBody:
                OnBody();
                break;

            case HandStates.Thrown:
                Thrown();
                break;

            case HandStates.Returning:
                Returning();
                break;
        }
    }

    void OnBody()
    {
       

        
        
        // if hand has no grabbedObj
        if (grabbedObj == null)
        {
            // if mouse left button is clicked
            if (Input.GetMouseButtonDown(0))
            {
                this.transform.parent = null; // detatch hand from the handHolder
                thrownInitialPos = this.transform.position; // save thrownPosition to calculate distance travelled by the hand
                handState = HandStates.Thrown;
            }
        }
        else
        {
             grabbedObj.GrabbedBehavior(this.transform.position.y); // call GrabbedBehavior update function of the grabbedObj

            // if mouse right button is held pressed
            if (Input.GetMouseButton(1))
            {
                currentChargePitch -= valuesChangingDir * Time.deltaTime * pitchIncreaseRate;
                this.transform.forward = (Quaternion.AngleAxis(currentChargePitch, this.transform.right) * this.transform.parent.forward).normalized;

                currentThrowingVelocity += valuesChangingDir * Time.deltaTime * pitchIncreaseRate;

                if (currentChargePitch < -maxChargePitch || currentChargePitch > 0 || currentThrowingVelocity < 0 || currentThrowingVelocity > maxThrowingVelocity)
                {
                    currentChargePitch = Mathf.Clamp(currentChargePitch, -maxChargePitch, 0);
                    currentThrowingVelocity = Mathf.Clamp(currentThrowingVelocity, 0, maxThrowingVelocity);

                    valuesChangingDir *= -1;
                }

                chargeLineRenderer.UpdatePointsPos(true);
            }
            // if mouse right button is released or mouse left button is clicked
            else if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonDown(0))
            {
                grabbedObj.ThrowAction((currentThrowingVelocity > 0 ? currentThrowingVelocity : maxThrowingVelocity) * this.transform.forward); // throw grabbedObj
                grabbedObj = null;

                this.transform.forward = handHolder.forward;
                currentChargePitch = 0;
                currentThrowingVelocity = 0;

                chargeLineRenderer.UpdatePointsPos(false);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Transform playerTransform = handHolder.parent;
                grabbedObj.GetComponent<Enemy>().ConsumeAction(playerTransform);
                
                 Instantiate(poofSmokePrefab, playerTransform.GetComponentInChildren<SkinnedMeshRenderer>().bounds.center, Quaternion.identity, null);
                // GameObject specialMechanincObj = Instantiate(grabbedObj.GetComponent<GrabbableEnemy>().specialMechanicObjPrefab, playerTransform.position, playerTransform.rotation, playerTransform);
                // GameObject.DestroyImmediate(grabbedObj.gameObject);
            }
        }
    }

    void Thrown()
    {
        this.transform.Translate(this.transform.forward * Time.deltaTime * speed, Space.World); // move hand with time

        // if hand has travelled maxDist
        if (Vector3.Distance(thrownInitialPos, this.transform.position) > maxDist)
        {
            handState = HandStates.Returning;
        }
    }

    void Returning()
    {
        float distance = Vector3.Distance(this.transform.position, handHolder.position); // distance between hand and handHolder

        float translation = speed * Time.deltaTime; // small translation to be made by the hand on this update call
        // if distance between hand and handHolder is still big enough
        if (distance > translation)
        {
            Debug.DrawLine(this.transform.position, handHolder.position, Color.white);
            Vector3 returningDir = (handHolder.position - this.transform.position).normalized; // direction from hand towards the handHolder
            this.transform.Translate(returningDir * translation, Space.World); // move hand with time, towards the handHolder
        }
        else
        {
            // when distance is finally too small, instantly return hand to handHolder
            this.transform.parent = handHolder;
            this.transform.localRotation = Quaternion.identity;
            this.transform.localPosition = Vector3.zero;
            handState = HandStates.OnBody;
        }
    }

   
    
    void OnTriggerStay(Collider other)
     {
         // Debug.Log(other.gameObject.name);


         Enemy temp = other.GetComponent<Enemy>();

         if (temp != null && temp.canBeGrabbed && handState != HandStates.OnBody)
         {
             temp.GrabAction( 0.5f, grabbablePlaceHolder, grabbablePlaceHolder.rotation, grabbablePlaceHolder.position);

             
             if (grabbablePlaceHolder.childCount > 1)
             {

                 grabbedObj = grabbablePlaceHolder.GetChild(1).GetComponent<Enemy>();
             }

         }

         if(!other.CompareTag("Player"))
         handState = HandStates.Returning; // return hand regardless of what it collided with
     }
    
}


