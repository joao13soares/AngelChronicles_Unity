using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleOffsetCamera : MonoBehaviour
{
    [SerializeField] private Transform TransformToFollow;
    [SerializeField] private Vector3 Offset;
    
    

    // Update is called once per frame
    void Update()
    {
        this.transform.position = TransformToFollow.position + Offset;
    }
}
