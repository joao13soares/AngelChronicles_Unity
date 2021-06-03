using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public bool canBeGrabbed;
    Vector3 defaultScale;

     protected IThrowable throwAction;
     protected IGrabbable grabAction;
     protected IConsumable consumableAction;

    [SerializeField] private GameObject prefabForConsume;

    public delegate void EnemyLifeCycle();
    public event EnemyLifeCycle EnemyDied;
    
    
    
    // Floating Animation Variables
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float floatingSpeed;
    [SerializeField] private float maxHeightOffset;

    private int dir = 1;
     private float currentHeightOffset;




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

    }


    // called by the HandBehavior script whenever this instance of Grabbable is the grabbedObj
    public void GrabbedBehavior(float holderHeight)
    {
        // rotation animation
        this.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // floating animation - calculate currentHeightOffset
        currentHeightOffset += dir * floatingSpeed * Time.deltaTime;

        // floating animation - add currentHeightOffset
        this.transform.position = new Vector3(
            this.transform.position.x,
            holderHeight + currentHeightOffset,
            this.transform.position.z);

        // floating animation - check change of direction
        if ((currentHeightOffset < 0 && dir == -1) ||
            (currentHeightOffset >= maxHeightOffset && dir == 1))
        {
            dir *= -1;
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        Enemy temp = other.gameObject.GetComponent<Enemy>();


        if (temp == null) return;
        
        this.EnemyDied?.Invoke();
        Destroy(this.gameObject);

        
    }
    
    
    
}
