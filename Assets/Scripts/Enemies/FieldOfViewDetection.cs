using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewDetection : MonoBehaviour
{   
    private Transform enemyTransform;
    private Transform playerTransform;
    private Transform playerPositionCorrectedTransform;
    [SerializeField] private LayerMask enemyLayer;

    public float fovAngle;
    public float detectRange;


    public bool isPlayerDetected => IsInDetectionRange && IsAngleValid() && IsVisible();
    private bool IsInDetectionRange => (playerTransform.position - enemyTransform.position).magnitude <= detectRange;

    private void Awake()
    {
        enemyTransform = this.transform;
        playerPositionCorrectedTransform = GameObject.Find("CameraViewPoint").transform;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private bool IsAngleValid()
    {
        Vector3 toPlayer = playerPositionCorrectedTransform.position - enemyTransform.position;
        toPlayer.y *= 0f; // Dont care about Y, we make it 2D 

        float angleBetween = Vector3.Angle(enemyTransform.forward, toPlayer);

        if (angleBetween < -fovAngle / 2 || angleBetween > fovAngle / 2) return false;

        return true;
    }

    private bool IsVisible()
    {
        RaycastHit hit;

        Vector3 toPlayer = playerPositionCorrectedTransform.position - enemyTransform.position;



        if (Physics.Raycast(enemyTransform.position, toPlayer, out hit, detectRange, ~enemyLayer))
        {
            // Debug.Log(hit.collider.gameObject.name);
            
            
            
            if (hit.collider.gameObject.transform == playerTransform)
            {
                return true;
            }
        }

        return false;
    }
    
    // private void OnDrawGizmos()
    // {
    //     
    //     Gizmos.color = Color.cyan;
    //     Gizmos.DrawWireSphere(enemyTransform.position, detectRange);
    //
    //     Vector3 fovLine1 = Quaternion.AngleAxis(-fovAngle / 2, Vector3.up) * enemyTransform.forward * detectRange;
    //     Vector3 fovLine2 = Quaternion.AngleAxis(fovAngle / 2, Vector3.up) * enemyTransform.forward * detectRange;
    //
    //
    //     Gizmos.color = Color.yellow;
    //
    //     Gizmos.DrawRay(enemyTransform.position, fovLine1);
    //     Gizmos.DrawRay(enemyTransform.position, fovLine2);
    //
    //     Vector3 toPlayer = playerPositionCorrectedTransform.position - enemyTransform.position;
    //     toPlayer = Vector3.ClampMagnitude(toPlayer, detectRange);
    //
    //     
    //     
    //
    //     if (isPlayerDetected)
    //         Gizmos.color = Color.green;
    //
    //     else Gizmos.color = Color.red;
    //
    //     Gizmos.DrawRay(enemyTransform.position, toPlayer);
    // }

}