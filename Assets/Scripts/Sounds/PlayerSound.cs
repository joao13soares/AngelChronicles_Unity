using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    
    
    [SerializeField] private AudioClip leftFootSFX;
    [SerializeField] private AudioClip rightFootSFX;
    
    void PlayRightFootSound() => this.GetComponent<AudioSource>().PlayOneShot(rightFootSFX);

    void PlayLeftFootSound() => this.GetComponent<AudioSource>().PlayOneShot(leftFootSFX);
  
}
