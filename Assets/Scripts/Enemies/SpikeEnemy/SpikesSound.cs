using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesSound : MonoBehaviour
{
    [SerializeField] AudioClip spikesSFX;
    void PlaySpikesSound() => this.GetComponent<AudioSource>().PlayOneShot(spikesSFX);
}
