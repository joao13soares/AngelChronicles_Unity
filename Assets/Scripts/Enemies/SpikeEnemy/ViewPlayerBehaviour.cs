using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FieldOfViewDetection))]
public class ViewPlayerBehaviour : MonoBehaviour
{

    private FieldOfViewDetection FOVDetection;
    private Enemy enemy;
    
    
    // Start is called before the first frame update
    void Start()
    {
        FOVDetection = this.GetComponent<FieldOfViewDetection>();
        enemy = this.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FOVDetection.isPlayerDetected);
    }
}
