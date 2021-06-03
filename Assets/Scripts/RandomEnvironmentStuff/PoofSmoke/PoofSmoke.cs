using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoofSmoke : MonoBehaviour
{
    [SerializeField] float smokeRadius; // 1.5
    [SerializeField] int numSmokeSpheres; // 20
    [SerializeField] GameObject smokeSpherePrefab;

    void Start()
    {
        for (int i = 0; i < numSmokeSpheres; i++)
        {
            Vector3 randPos = this.transform.position + Random.insideUnitSphere * smokeRadius;
            Instantiate(smokeSpherePrefab, randPos, Quaternion.identity, this.transform);
        }
    }

    void Update()
    {
        if (this.transform.childCount <= 0)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
