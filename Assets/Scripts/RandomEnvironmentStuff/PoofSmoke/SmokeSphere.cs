using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeSphere : MonoBehaviour
{
    float maxLifeTime, currentLifeTime;
    float disappearingRate;

    void Start()
    {
        this.transform.localScale *= Random.Range(1.5f, 3.0f);

        maxLifeTime = Random.Range(1.0f, 2.0f);
        disappearingRate = Random.Range(0.975f, 0.995f);
    }

    void Update()
    {
        currentLifeTime += Time.deltaTime;

        if (currentLifeTime >= 0.05f)
        {
            this.transform.localScale *= disappearingRate;
        }

        if (this.transform.localScale.x <= 0.5f || currentLifeTime >= maxLifeTime)
        {
            GameObject.DestroyImmediate(this.gameObject);
        }
    }
}
