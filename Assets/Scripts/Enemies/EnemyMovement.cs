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
    
    
    
    public void SetPath(Transform[] path)
     {
        pathPoints = path;
    }
    
    
    void Start()
    {
        currentMovState = EnemyMovState.ROTATING;
    }

    // Update is called once per frame
    void Update()
    {
        if (pathPoints.Length == 0) return;
        
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
        
        
        Vector3 correctedTargetPos = pathPoints[currentTargetIndex].position;
        // correctedTargetPos.y = transform.position.y;
        
        
        Vector3 desiredForward = (correctedTargetPos - transform.position).normalized;

        float targetDirectionTolerance = 0.001f;
        



        float angleToTurn = 90f;
        
        
        
        if (Vector3.Dot(desiredForward, transform.forward) >= 1f- targetDirectionTolerance)
        {
            
            currentMovState = EnemyMovState.WALKING;
            return;
        }

        transform.Rotate(transform.up,angleToTurn * Time.deltaTime,Space.World);
        
        
        // transform.forward = Vector3.Slerp(transform.forward,desiredForward ,0.05f);
    }

    private void WalkTowardsTarget()
    {
        float targetArea = 0.5f;
        Vector3 projectedTarget = pathPoints[currentTargetIndex].position;
        // projectedTarget.y *= 0;
        Vector3 currentPos = transform.position;
        // currentPos.y *= 0;

        float currentDistance = (projectedTarget - currentPos).magnitude;

         
         
        if (currentDistance <= targetArea)
        {
            currentTargetIndex = ChangeTargetIndex();
            currentMovState = EnemyMovState.ROTATING;
            return;

        }

        float movForce = 1.5f;
        transform.position += transform.forward * movForce * Time.deltaTime;
    }
    
    private int ChangeTargetIndex()
    {
        
        if (currentTargetIndex + 1 >= pathPoints.Length)
            return currentTargetIndex = 0;

        return currentTargetIndex + 1;
    }
}