using UnityEngine;

public class WaterBob : MonoBehaviour
{
    [SerializeField]
    float height = 0.1f;

    [SerializeField]
    float rotation = 1f;

    [SerializeField]
    float rotationP = 1;

    [SerializeField]
    float period = 1;

    private Vector3 initialPosition;
    private Vector3 InitialRotation;
    private float offset;

    private void Awake()
    {
        initialPosition = transform.position;
        InitialRotation = transform.rotation.eulerAngles;

        offset = 1 - (Random.value * 2);
    }

    private void Update()
    {
        transform.position = initialPosition - Vector3.up * Mathf.Sin((Time.time + offset) * period) * height;
        float eulerX = InitialRotation.x - Vector3.right.x * Mathf.Sin((Time.time + offset) * rotationP) * rotation;

        float eulerZ = InitialRotation.x - Vector3.forward.z * Mathf.Sin((Time.time + offset) * rotationP) * rotation;
        transform.eulerAngles = new Vector3(eulerX, transform.eulerAngles.y, eulerZ);
    }
}
