using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHouseFade : MonoBehaviour
{
    [SerializeField] private float quantityToAlter;
    private float direction;

    [SerializeField]private MeshRenderer lightHouseRenderer;
    

    

    void ChangeDirection(float newDir) => direction = newDir;


  
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        direction = -1;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        direction = 1;
    }

    private void Update()
    {
        float newAlpha = Mathf.Clamp01(lightHouseRenderer.material.GetFloat("_Alpha") +
                                       direction * Time.deltaTime * quantityToAlter);
        
        lightHouseRenderer.material.SetFloat("_Alpha", newAlpha);
        
        
    }
    
    
    
}
