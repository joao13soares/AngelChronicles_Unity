// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class EnemyMovement : MonoBehaviour
// {
//
//     private enum EnemyMovState
//     {
//         WALKING,
//         ROTATING
//
//     };
//     
//     [SerializeField] private List<Transform> pathPoints;
//     private int currentTargetIndex;
//
//     private EnemyMovState currentMovState;
//     
//     void Start()
//     {
//         currentTargetIndex = 0;
//         currentMovState = EnemyMovState.ROTATING;
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         
//         Debug.Log(pathPoints[currentTargetIndex].position);
//         switch (currentMovState)
//         {
//             case EnemyMovState.WALKING:
//                 WalkTowardsTarget();
//                 break;
//             case EnemyMovState.ROTATING:
//                 RotateTowardsTarget();
//                 break;
//             
//         }
//     }
//
//
//     public void SetPath(List<Transform> path)
//     {
//         pathPoints = path;
//     }
//
//     private void RotateTowardsTarget()
//     {
//        
//         Vector3 desiredForward = (pathPoints[currentTargetIndex].position - transform.position).normalized;
//
//         float targetDirectionTolerance = 0.001f;
//         
//          // Debug.Log(Vector3.Dot(transform.forward, desiredForward));
//         
//         if (Vector3.Dot(desiredForward, transform.forward) >= 1f- targetDirectionTolerance)
//         {
//             Debug.Log(currentTargetIndex);
//             // Debug.Log("PODE ANDAR");
//             currentMovState = EnemyMovState.WALKING;
//             return;
//         }
//
//         
//         transform.forward = Vector3.Slerp(transform.forward,desiredForward ,0.01f);
//         
//     }
//
//     private void WalkTowardsTarget()
//     {
//         float targetArea = 0.5f;
//
//
//         // Vector3 target2DConvertion = pathPoints[currentTargetIndex].position;
//         // target2DConvertion.y*= 0;
//         //
//         // Vector3 currentPos2dConvertion = transform.position;
//         // currentPos2dConvertion.y*= 0;
//         
//          float currentDistance = (pathPoints[currentTargetIndex].position - transform.position).magnitude;
//
//         // float currentDistance = (target2DConvertion- currentPos2dConvertion).magnitude;
//         
//         // Debug.Log(currentDistance<= targetArea);
//         
//         if (currentDistance <= targetArea)
//         {
//            
//             ChangeTargetIndex();
//             currentMovState = EnemyMovState.ROTATING;
//             return;
//
//         }
//
//         float movForce = 1f;
//         transform.position += transform.forward * movForce * Time.deltaTime;
//     }
//     
//     private int ChangeTargetIndex()
//     {
//         if (currentTargetIndex + 1 >= pathPoints.Count)
//             return 0;
//
//         return currentTargetIndex++;
//     }
// }
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
        correctedTargetPos.y = transform.position.y;
        
        
        Vector3 desiredForward = (correctedTargetPos - transform.position).normalized;

        float targetDirectionTolerance = 0.00005f;
        
        // Debug.Log(Mathf.Abs(Vector3.Dot(transform.forward, desiredForward)));


        float angleToTurn = 90f;
        
        
        
        if (Vector3.Dot(desiredForward, transform.forward) >= 1f- targetDirectionTolerance)
        {
            
            currentMovState = EnemyMovState.WALKING;
            return;
        }

        transform.Rotate(transform.up,angleToTurn * Time.deltaTime,Space.Self);
        
        
        // transform.forward = Vector3.Slerp(transform.forward,desiredForward ,0.05f);
    }

    private void WalkTowardsTarget()
    {
        float targetArea = 0.5f;
        Vector3 projectedTarget = pathPoints[currentTargetIndex].position;
        projectedTarget.y *= 0;
        Vector3 currentPos = transform.position;
        currentPos.y *= 0;

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