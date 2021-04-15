using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] float lifetime;
    float currentLifetime;

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
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy();
    }

    void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }
}
