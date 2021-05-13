using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    private enum EnemyMovState
    {
        WALKING,
        ROTATING

    };
    
    [SerializeField] private Transform[] pathPoints;
    private int currentTargetIndex;

    private EnemyMovState currentMovState;
    
    void Start()
    {
        currentMovState = EnemyMovState.ROTATING;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentMovState)
        {
            case EnemyMovState.WALKING:
                WalkTowardsTarget();
                break;
            case EnemyMovState.ROTATING:
                RotateTowardsTarget();
                break;
            
        }
    }

    
    
    
    private void RotateTowardsTarget()
    {
        Vector3 desiredForward = (pathPoints[currentTargetIndex].position - transform.position).normalized;

        float targetDirectionTolerance = 0.001f;
        
        // Debug.Log(Mathf.Abs(Vector3.Dot(transform.forward, desiredForward)));
        
        if (Mathf.Abs(Vector3.Dot(desiredForward, transform.forward)) >= 1f- targetDirectionTolerance)
        {
            Debug.Log("PODE ANDAR");
            currentMovState = EnemyMovState.WALKING;
            return;
        }

        
        transform.forward = Vector3.Slerp(transform.forward,desiredForward ,0.01f);
    }

    private void WalkTowardsTarget()
    {
        float targetArea = 0.5f;

        float currentDistance = (pathPoints[currentTargetIndex].position - transform.position).magnitude;

        Debug.Log(currentDistance);
        if (currentDistance <= targetArea)
        {
            ChangeTargetIndex();
            currentMovState = EnemyMovState.ROTATING;
            return;

        }

        float movForce = 1f;
        transform.position += transform.forward * movForce * Time.deltaTime;
    }
    
    private int ChangeTargetIndex()
    {
        
        if (currentTargetIndex + 1 >= pathPoints.Length)
            return 0;

        return currentTargetIndex++;
    }
}
