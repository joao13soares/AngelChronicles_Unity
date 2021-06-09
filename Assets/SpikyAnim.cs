using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyAnim : MonoBehaviour
{
    [SerializeField]
    Animator bodyAnimator;
    [SerializeField]
    Animator spikesAnimator;
    
    enum SpikyState
    {
        Retract,
        Opening
    }


    // Update is called once per frame
    void Update()
    {
        bool isPlayerDetected = this.GetComponent<FieldOfViewDetection>().isPlayerDetected;
        Debug.Log(isPlayerDetected);

        if(isPlayerDetected)
        {
            bodyAnimator.SetInteger("animState", (int)SpikyState.Opening);
            spikesAnimator.SetInteger("animState", (int)SpikyState.Opening);
        }
        else
        {
            bodyAnimator.SetInteger("animState", (int)SpikyState.Retract);
            spikesAnimator.SetInteger("animState", (int)SpikyState.Retract);
        }
    }
}
