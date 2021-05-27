using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    enum PlayerAnimState
    {
        Idle0,
        Idle1,
        Idle2,
        Running,
        Jumping,
        Falling
    }

    bool previousStateWasIdle = true;

    [SerializeField] Animator bodyAnimator;
    [SerializeField] Animator handAnimator;

    void Update()
    {
        float playerVelY = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity.y;
        bool playerIsMagnetized = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().isMagnetized;
        bool playerIsUsingEnemyMechanic = GameObject.FindGameObjectWithTag("Player").transform.childCount > 3;
        
        
        if ((playerVelY > Mathf.Pow(10, -4) && !playerIsMagnetized) || (playerVelY < -Mathf.Pow(10, -4) && playerIsMagnetized))
        {
            bodyAnimator.SetInteger("animState", (int) PlayerAnimState.Jumping);
            handAnimator.SetInteger("animState", (int)PlayerAnimState.Jumping);
            previousStateWasIdle = false;
        }
        else if ((playerVelY < -Mathf.Pow(10, -4) && !playerIsMagnetized) || (playerVelY > Mathf.Pow(10, -4) && playerIsMagnetized))
        {
            bodyAnimator.SetInteger("animState", (int)PlayerAnimState.Falling);
            handAnimator.SetInteger("animState", (int)PlayerAnimState.Falling);
            previousStateWasIdle = false;
        }
        else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && !playerIsUsingEnemyMechanic)
        {
            bodyAnimator.SetInteger("animState", (int)PlayerAnimState.Running);
            handAnimator.SetInteger("animState", (int)PlayerAnimState.Running);
            previousStateWasIdle = false;
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
                        bodyAnimator.SetInteger("animState", (int)PlayerAnimState.Idle1);
                        handAnimator.SetInteger("animState", (int)PlayerAnimState.Idle1);
                        break;
                    case 2:
                        bodyAnimator.SetInteger("animState", (int)PlayerAnimState.Idle2);
                        handAnimator.SetInteger("animState", (int)PlayerAnimState.Idle2);
                        break;
                    default:
                        bodyAnimator.SetInteger("animState", (int)PlayerAnimState.Idle0);
                        handAnimator.SetInteger("animState", (int)PlayerAnimState.Idle0);
                        break;
                }
            }

            previousStateWasIdle = true;
        }


    }
}
