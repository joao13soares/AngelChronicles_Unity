using System;
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

    public delegate void EnemyLifeCycle();
    public event EnemyLifeCycle EnemyDied;


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
        this.EnemyDied?.Invoke();

    }

    public void ConsumeAction(Transform playerTransform)
    {
        consumableAction.ConsumeAction(playerTransform, this.prefabForConsume);
        Destroy(this.gameObject);
        this.EnemyDied?.Invoke();
    }

    public void GrabAction(float scalingFactor, Transform handTransform, Quaternion handRotation, Vector3 handPosition)
    {
        grabAction.GrabAction(this.gameObject, scalingFactor, handTransform, handRotation, handPosition, out defaultScale);
        canBeGrabbed = false;

    }

    private void OnCollisionEnter(Collision other)
    {
        Enemy temp = other.gameObject.GetComponent<Enemy>();


        if (temp == null) return;
        
        this.EnemyDied?.Invoke();
        Destroy(this.gameObject);

        
    }
    
    
    
}
