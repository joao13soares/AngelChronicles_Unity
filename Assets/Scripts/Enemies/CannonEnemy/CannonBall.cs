using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] float lifetime;
    float currentLifetime;

    [SerializeField] GameObject cannonBallPredictObj;
    [SerializeField] LayerMask ignorableLayers;
    public Vector3 cannonBallPredict;

   

     public void GetPrediction(Vector3 predictionPos)
     {
         cannonBallPredict = predictionPos;
         cannonBallPredictObj.transform.position = predictionPos;

     }
    
    void Update()
    {
        if (currentLifetime >= lifetime)
        {
            Destroy();
        }
        else
        {
            currentLifetime += Time.deltaTime;

            Debug.DrawRay(this.transform.position, this.transform.forward, this.GetComponent<Renderer>().material.color);
        }

        cannonBallPredictObj.transform.position = cannonBallPredict;
    }

    void UpdatePredictPosition()
    {
        RaycastHit hit;

        if (Physics.Raycast(cannonBallPredictObj.transform.position, Vector3.down, out hit, 50, ~ignorableLayers))
        {
            cannonBallPredictObj.SetActive(true);
            cannonBallPredictObj.transform.rotation = Quaternion.identity;
            cannonBallPredictObj.transform.position = hit.point + Vector3.up * 0.2f;
        }
        else
        {
            cannonBallPredictObj.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy();
    }

    void Destroy()
    {
        GameObject.Destroy(this.gameObject);
        GameObject.Destroy(cannonBallPredictObj);
    }
}