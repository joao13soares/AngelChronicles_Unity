using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public bool canBeGrabbed;
    Vector3 defaultScale;

    [SerializeField] protected IThrowable throwAction;
    [SerializeField] protected IGrabbable grabAction;
    [SerializeField] protected IConsumable consumableAction;

    [SerializeField] private GameObject prefabForConsume;


    void Awake()
    {
        defaultScale = this.gameObject.transform.localScale;
        throwAction = this.GetComponent<IThrowable>();
        grabAction = this.GetComponent<IGrabbable>();
        consumableAction = this.GetComponent<IConsumable>();

    }

    public void ThrowAction(Vector3 velocity)
    {
        throwAction.ThrowAction(this.gameObject, velocity);
    }

    public void ConsumeAction(Transform playerTransform)
    {
        consumableAction.ConsumeAction(playerTransform, this.prefabForConsume);
        Destroy(this.gameObject);
    }

    public void GrabAction(float scalingFactor, Transform handTransform, Quaternion handRotation, Vector3 handPosition)
    {
        grabAction.GrabAction(this.gameObject, scalingFactor, handTransform, handRotation, handPosition, out defaultScale);
        canBeGrabbed = false;

    }

}
