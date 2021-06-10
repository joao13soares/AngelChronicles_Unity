using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyAnim : MonoBehaviour
{
    [SerializeField] Animator spikesAnimator;
    
    enum SpikyState
    {
        Retract,
        Opening
    }

    // Update is called once per frame
    void Update()
    {
        bool isPlayerDetected = this.GetComponent<FieldOfViewDetection>().isPlayerDetected;

        if(isPlayerDetected)
        {
            spikesAnimator.SetInteger("animState", (int)SpikyState.Opening);
        }
        else
        {
            spikesAnimator.SetInteger("animState", (int)SpikyState.Retract);
        }
    }
}
