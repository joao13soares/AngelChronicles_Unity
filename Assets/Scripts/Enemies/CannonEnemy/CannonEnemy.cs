using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonEnemy : MonoBehaviour
{
    GameObject player;
    [SerializeField] float maxViewAngle;
    [SerializeField] float maxViewDistance;
    [SerializeField] LayerMask ignorableLayers;

    Vector3 defaultForward;
    [SerializeField] float chasingVelocity;
    [SerializeField] float timeToReachPlayer;

    [SerializeField] float shootingCooldown;
    float timeForNextShoot;

    GameObject cannonBall;
    [SerializeField] GameObject cannonBallPrefab;
    [SerializeField] Transform cannonBallExitPoint;

    // [SerializeField] GameObject targetPredictObj;

    void Start()
    {
        player = GameObject.Find("Player");

        defaultForward = this.transform.forward;
    }

    void Update()
    {
        Vector3 horDefaultForward = defaultForward; horDefaultForward.y = 0; horDefaultForward.Normalize();
        Vector3 rightRayDir = (Quaternion.AngleAxis(maxViewAngle * 0.5f, Vector3.up) * horDefaultForward).normalized;
        Vector3 leftRayDir = (Quaternion.AngleAxis(-maxViewAngle * 0.5f, Vector3.up) * horDefaultForward).normalized;
        Debug.DrawRay(this.transform.position, rightRayDir * maxViewDistance);
        Debug.DrawRay(this.transform.position, leftRayDir * maxViewDistance);

        bool detected = false;
        if (Vector3.Distance(this.transform.position, player.transform.position) <= maxViewDistance)
        {
            Vector3 horVectToPlayer = player.transform.position - this.transform.position; horVectToPlayer.y = 0; horVectToPlayer.Normalize();

            if (Vector3.Angle(horDefaultForward, horVectToPlayer) <= maxViewAngle * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(this.transform.position, player.transform.position - this.transform.position, out hit, maxViewDistance, ~ignorableLayers))
                {
                    if (hit.collider.tag == player.tag)
                    {
                        detected = true;
                    }
                }
            }
        }

        if (detected)
        {
            Debug.DrawRay(this.transform.position, player.transform.position - this.transform.position, Color.red);
            //this.transform.GetChild(0).GetComponent<Renderer>().material.color = player.GetComponent<Renderer>().material.color;

            Vector3 targetPosPredict = player.transform.position + player.transform.forward * player.GetComponent<Rigidbody>().velocity.magnitude;
            // targetPredictObj.SetActive(true); targetPredictObj.transform.position = targetPosPredict;

            Vector3 shootingVelocityVec = CalcVelocityVector(targetPosPredict);

            this.transform.forward = Vector3.Lerp(this.transform.forward, shootingVelocityVec, Time.deltaTime * chasingVelocity);
            //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(shootingVelocityVec), Time.deltaTime * chasingVelocity);
            //this.transform.rotation = Quaternion.LookRotation(shootingVelocityVec);

            if (timeForNextShoot <= 0)
            {
                
                cannonBall = Instantiate(cannonBallPrefab, cannonBallExitPoint.position, cannonBallExitPoint.rotation, null);
                cannonBall.GetComponent<CannonBall>().GetPrediction(targetPosPredict + Vector3.up * 0.1f);
                cannonBall.GetComponent<Rigidbody>().velocity = shootingVelocityVec;

                timeForNextShoot = shootingCooldown;
            }
            else
            {
                timeForNextShoot -= Time.deltaTime;
            }
        }
        else
        {
            //this.transform.GetChild(0).GetComponent<Renderer>().material.color = this.GetComponent<Renderer>().material.color;
            // targetPredictObj.SetActive(false);

            this.transform.forward = Vector3.Lerp(this.transform.forward, defaultForward, Time.deltaTime);

            timeForNextShoot = 0.5f;
        }
    }

    Vector3 CalcVelocityVector(Vector3 targetPos)
    {
        Vector3 dist = targetPos - cannonBallExitPoint.position;

        Vector3 distXZ = dist; distXZ.y = 0;
        float vXZ0 = distXZ.magnitude / timeToReachPlayer;

        float distY = dist.y;
        float vY0 = distY / timeToReachPlayer + 0.5f * Mathf.Abs(Physics.gravity.y) * timeToReachPlayer;

        Vector3 v0 = distXZ.normalized * vXZ0; v0.y = vY0;

        return v0;
    }
}
