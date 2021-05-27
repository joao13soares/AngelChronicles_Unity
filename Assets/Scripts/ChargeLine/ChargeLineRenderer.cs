using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeLineRenderer : MonoBehaviour
{
    GameObject[] pointsObjs;
    [SerializeField] int numPoints;

    [SerializeField] GameObject pointObjPrefab;

    [SerializeField] float refreshRate;
    float time;

    [SerializeField] HandBehavior handBehavior;

    void Start()
    {
        LoadPointsObjs();

        time = refreshRate;
    }

    public void UpdatePointsPos(bool toggle)
    {
        for (int i = 0; i < numPoints; i++)
        {
            pointsObjs[i].SetActive(toggle);
        }

        if (toggle)
        {
            time += Time.deltaTime;
            if (time >= refreshRate)
            {
                Vector3 v0 = handBehavior.currentThrowingVelocity * handBehavior.transform.forward;
                CalcPointsPos(v0, 0);

                time = 0;
            }
        }
        else
        {
            time = refreshRate;
        }
    }

    void LoadPointsObjs()
    {
        pointsObjs = new GameObject[numPoints];

        for (int i = 0; i < numPoints; i++)
        {
            pointsObjs[i] = Instantiate(pointObjPrefab, Vector3.zero, Quaternion.identity, this.transform);
            pointsObjs[i].name = "Point" + i;
            pointsObjs[i].SetActive(false);
        }
    }

    // https://www.youtube.com/watch?v=iLlWirdxass
    void CalcPointsPos(Vector3 v0, float y0)
    {
        float g = Mathf.Abs(Physics.gravity.y);

        float radAngle = Vector3.Angle(v0, handBehavior.transform.parent.forward) * Mathf.Deg2Rad;

        if (v0.magnitude > 0 && radAngle != 0)
        {
            //float maxDistXZ = Mathf.Pow(v0.magnitude, 2) * Mathf.Sin(2 * radAngle) / g;
            float maxDistXZ = (v0.magnitude * Mathf.Cos(radAngle) / g) * (v0.magnitude * Mathf.Sin(radAngle) + Mathf.Sqrt(Mathf.Pow(v0.magnitude * Mathf.Sin(radAngle), 2) + 2 * g * y0));

            for (int i = 0; i < numPoints; i++)
            {
                float distXZi = maxDistXZ / numPoints * (i + 1);
                float distYi = distXZi * Mathf.Tan(radAngle) - g * Mathf.Pow(distXZi, 2) / (2 * Mathf.Pow(v0.magnitude * Mathf.Cos(radAngle), 2));

                Vector3 v0XZ = v0; v0XZ.y = 0;
                Vector3 disti = v0XZ.normalized * distXZi; disti.y = distYi;

                pointsObjs[i].transform.position = this.transform.position + disti;
            }
        }
    }
}
