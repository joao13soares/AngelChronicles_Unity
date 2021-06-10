using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAnimation : MonoBehaviour
{
    [SerializeField]
    Animator buildingAnim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buildingAnim.SetTrigger("Falling");
        }
    }
}
