// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class HandBehavior : MonoBehaviour
// {
//     enum HandStates
//     {
//         OnBody,
//         Thrown,
//         Returning
//     }
//
//     HandStates handState;
//
//
//
//     [SerializeField] float speed;
//     [SerializeField] float maxDist;
//     [SerializeField] float aimAssistRadius;
//     [SerializeField] Transform handHolder;
//     
//     Vector3 thrownPosition;
//     Enemy grabbedObj;
//
//     [SerializeField] float throwingForce;
//
//     void Update()
//     {
//         switch (handState)
//         {
//             case HandStates.OnBody:
//                 OnBody();
//                 break;
//
//             case HandStates.Thrown:
//                 Thrown();
//                 break;
//
//             case HandStates.Returning:
//                 Returning();
//                 break;
//         }
//     }
//
//     void OnBody()
//     {
//         //// ANIMATION
//
//         //// if hand has grabbedObj
//         //if (grabbedObj != null)
//         //{
//         //    grabbedObj.GrabbedBehavior(this.transform.position.y); // call GrabbedBehavior update function of the grabbedObj
//         //}
//
//         /////////////////////////////////////////
//
//
//         
//
//
//
//         // if mouse left button is clicked
//         if (Input.GetMouseButtonDown(0))
//         {
//             // if hand has no grabbedObj
//             if (grabbedObj == null)
//             {
//                 this.transform.parent = null; // detatch hand from the handHolder
//                 thrownPosition = this.transform.position; // save thrownPosition to calculate distance travelled by the hand
//                 this.GetComponent<Collider>().enabled = true;
//                 handState = HandStates.Thrown;
//             }
//             else
//             {
//                 grabbedObj.ThrowAction(throwingForce, this.transform.forward); // throw grabbedObj
//                 grabbedObj = null;
//             }
//         }
//
//
//
//
//
//         // if mouse right button is held pressed
//         if (Input.GetMouseButton(1))
//         {
//             Vector3 boxCastCenter = this.transform.position + this.transform.lossyScale.z * 0.5f * this.transform.forward;
//             Vector3 boxCastHalfSize = new Vector3(aimAssistRadius, aimAssistRadius, 0);
//             RaycastHit boxCastHit;
//             if (Physics.BoxCast(boxCastCenter, boxCastHalfSize, this.transform.forward, out boxCastHit, Quaternion.identity, maxDist))
//             {
//                 if(boxCastHit.collider.tag == "Grabbable")
//                 {
//                     Debug.Log(boxCastHit.collider.name);
//                 }
//             }
//
//             Debug.DrawRay(boxCastCenter - boxCastHalfSize.x * this.transform.right + boxCastHalfSize.y * this.transform.up, this.transform.forward * maxDist, Color.blue);
//             Debug.DrawRay(boxCastCenter + boxCastHalfSize.x * this.transform.right + boxCastHalfSize.y * this.transform.up, this.transform.forward * maxDist, Color.blue);
//             Debug.DrawRay(boxCastCenter - boxCastHalfSize.x * this.transform.right - boxCastHalfSize.y * this.transform.up, this.transform.forward * maxDist, Color.red);
//             Debug.DrawRay(boxCastCenter + boxCastHalfSize.x * this.transform.right - boxCastHalfSize.y * this.transform.up, this.transform.forward * maxDist, Color.red);
//         }
//
//         if (Input.GetKey(KeyCode.E))
//         {
//             if (grabbedObj != null)
//             {
//                  Transform playerTransform = handHolder.parent;
//                 // GameObject specialMechanincObj = Instantiate(grabbedObj.GetComponent<GrabbableEnemy>().specialMechanicObjPrefab, playerTransform.position, playerTransform.rotation, playerTransform);
//                 // GameObject.DestroyImmediate(grabbedObj.gameObject);
//             }
//         }
//     }
//
//     void Thrown()
//     {
//         this.transform.Translate(this.transform.forward * Time.deltaTime * speed, Space.World); // move hand with time
//
//         // if hand has travelled maxDist
//         if (Vector3.Distance(thrownPosition, this.transform.position) > maxDist)
//         {
//             handState = HandStates.Returning;
//         }
//     }
//
//     void Returning()
//     {
//         float distance = Vector3.Distance(this.transform.position, handHolder.position); // distance between hand and handHolder
//
//         float translation = speed * Time.deltaTime; // small translation to be made by the hand on this update call
//         // if distance between hand and handHolder is still big enough
//         if (distance > translation)
//         {
//             Debug.DrawLine(this.transform.position, handHolder.position, Color.white);
//             Vector3 returningDir = (handHolder.position - this.transform.position).normalized; // direction from hand towards the handHolder
//             this.transform.Translate(returningDir * translation, Space.World); // move hand with time, towards the handHolder
//         }
//         else
//         {
//             // when distance is finally too small, instantly return hand to handHolder
//             this.transform.parent = handHolder;
//             this.transform.localRotation = Quaternion.identity;
//             this.transform.localPosition = Vector3.zero;
//             this.GetComponent<Collider>().enabled = false;
//             handState = HandStates.OnBody;
//         }
//     }
//
//     //void OnTriggerStay(Collider other) // it has to be OnTriggerStay (rather than OnTriggerEnter) because it could happen that the hand is already inside the enemy collider
//     //{
//     //    Debug.Log("Collided with: " + other.name);
//
//     //    if hand collided with a Grabbable object
//     //    if (other.tag == "Grabbable" && !other.GetComponent<Grabbable>().IsGrabbed && grabbedObj == null && handState == HandStates.Thrown)
//     //    {
//     //        Debug.Log("and Grabbed it!");
//
//     //        grabbedObj = other.GetComponent<Grabbable>(); // update reference
//     //        grabbedObj.Grab(0.5f, this.transform, this.transform.rotation, this.transform.position); // call Grab function of the grabbedObj
//     //    }
//
//     //    handState = HandStates.Returning; // return hand regardless of what it collided with
//     //}
//
//
//     void OnTriggerEnter(Collider other)
//     {
//         Debug.Log(other.gameObject.name);
//
//
//         Enemy temp = other.GetComponent<Enemy>();
//
//         if (temp != null && temp.canBeGrabbed)
//         {
//             Debug.Log("APANHA CARALHO");
//             temp.GrabAction( 0.5f, this.transform, this.transform.rotation, this.transform.position);
//             grabbedObj = temp;
//
//         }
//
//         handState = HandStates.Returning; // return hand regardless of what it collided with
//     }



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBehavior : MonoBehaviour
{
    enum HandStates
    {
        OnBody,
        Thrown,
        Returning
    }

    HandStates handState;
    [SerializeField] float speed;
    [SerializeField] float maxDist;
    [SerializeField] Transform handHolder;
    
    Vector3 thrownInitialPos;
     Enemy grabbedObj;

    [SerializeField] public float maxThrowingVelocity;
    public float currentThrowingVelocity;

    [SerializeField] float maxChargePitch;
    float currentChargePitch;
    [SerializeField] float pitchIncreaseRate;
    [SerializeField] ChargeLineRenderer chargeLineRenderer;

    int valuesChangingDir = 1;

    void Update()
    {
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
            // grabbedObj.GrabbedBehavior(this.transform.position.y); // call GrabbedBehavior update function of the grabbedObj

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

   
    
    void OnTriggerEnter(Collider other)
     {
         Debug.Log(other.gameObject.name);


         Enemy temp = other.GetComponent<Enemy>();

         if (temp != null && temp.canBeGrabbed)
         {
             Debug.Log("APANHA CARALHO");
             temp.GrabAction( 0.5f, this.transform, this.transform.rotation, this.transform.position);
             grabbedObj = temp;

         }

         handState = HandStates.Returning; // return hand regardless of what it collided with
     }
    
}

//
//
// }
