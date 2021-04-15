using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    private static int nextPriority;
    
    [SerializeField] private CinemachineVirtualCamera cam;
    // Start is called before the first frame update
  

    
    void ChangeCameraPriority()
    {
        nextPriority++;
        cam.Priority = nextPriority;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            ChangeCameraPriority();
    }
}
