using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    enum AnimState
    {
        Idle0,
        Idle1,
        Idle2,
        Running,
        Jumping,
        Falling,
        OnHeli,
        OnJumperDown,
        OnJumperUp,

        HandThrown,
        HandReturning,
        HandGrabbed
    }

    bool previousStateWasIdle = true;

    [SerializeField] Animator bodyAnimator;
    AnimState bodyAnimState;
    [SerializeField] Animator handAnimator;
    AnimState handAnimState;
    public Animator jumpConsumableAnimator;
     

    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] HandBehavior handBehavior;

    void Update()
    {
        //---------------------------------------------------------------

        float playerVelY = this.GetComponent<Rigidbody>().velocity.y;
        bool playerIsMagnetized = playerMovement.isMagnetized;
        bool playerIsUsingEnemyMechanic = this.transform.childCount > 4;
        bool playerIsOnHeli = playerMovement.isOnHeli;
        bool playerIsDoubleJumping = playerMovement.isDoubleJumping;

        if (playerIsUsingEnemyMechanic)
        {
            if (playerIsOnHeli)
            {
                bodyAnimState = AnimState.OnHeli;
                handAnimState = AnimState.OnHeli;
            }
            else if (playerIsDoubleJumping)
            {
                if (playerVelY < 0)
                {
                    bodyAnimState = AnimState.OnJumperDown;
                    handAnimState = AnimState.OnJumperDown;
                    jumpConsumableAnimator.SetInteger("animState", (int)AnimState.OnJumperDown);
                }
                else
                {
                    bodyAnimState = AnimState.OnJumperUp;
                    handAnimState = AnimState.OnJumperUp;
                    jumpConsumableAnimator.SetInteger("animState", (int)AnimState.OnJumperUp);
                }
            }
        }
        else if ((playerVelY > Mathf.Pow(10, -4) && !playerIsMagnetized) || (playerVelY < -Mathf.Pow(10, -4) && playerIsMagnetized))
        {
            bodyAnimState = AnimState.Jumping;
            handAnimState = AnimState.Jumping;
        }
        else if ((playerVelY < -Mathf.Pow(10, -4) && !playerIsMagnetized) || (playerVelY > Mathf.Pow(10, -4) && playerIsMagnetized))
        {
            bodyAnimState = AnimState.Falling;
            handAnimState = AnimState.Falling;
        }
        else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && !playerIsUsingEnemyMechanic)
        {
            bodyAnimState = AnimState.Running;
            handAnimState = AnimState.Running;
        }
        else
        {
            if (!previousStateWasIdle
                || (previousStateWasIdle && bodyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 > 0.99f && !bodyAnimator.IsInTransition(0)))
            {
                int r = (int)(Random.value * 8);
                switch (r)
                {
                    case 1:
                        bodyAnimState = AnimState.Idle1;
                        handAnimState = AnimState.Idle1;
                        break;
                    case 2:
                        bodyAnimState = AnimState.Idle2;
                        handAnimState = AnimState.Idle2;
                        break;
                    default:
                        bodyAnimState = AnimState.Idle0;
                        handAnimState = AnimState.Idle0;
                        break;
                }
            }
        }

        //---------------------------------------------------------------

        HandBehavior.HandStates handState = handBehavior.handState;
        bool hasGrabbedObj = handBehavior.grabbedObj != null;

        if (handState == HandBehavior.HandStates.Thrown)
        {
            handAnimState = AnimState.HandThrown;
        }
        else if (handState == HandBehavior.HandStates.Returning && !hasGrabbedObj)
        {
            handAnimState = AnimState.HandReturning;
        }
        else if ((handState == HandBehavior.HandStates.OnBody || handState == HandBehavior.HandStates.Returning) && hasGrabbedObj)
        {
            handAnimState = AnimState.HandGrabbed;
        }

        //---------------------------------------------------------------

        if (bodyAnimState.ToString().StartsWith("Idle")) previousStateWasIdle = true; else previousStateWasIdle = false;

        //---------------------------------------------------------------

        bodyAnimator.SetInteger("animState", (int)bodyAnimState);
        handAnimator.SetInteger("animState", (int)handAnimState);
    }
}
