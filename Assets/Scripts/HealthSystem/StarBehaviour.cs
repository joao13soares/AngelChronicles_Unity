using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBehaviour : MonoBehaviour
{


    [SerializeField] private HealthManager playerHealth;
    [SerializeField] private Transform starPlaceHolder;
    [SerializeField] private Vector3 offSet;
    [SerializeField] private float movementForce;


    [SerializeField] private Color[] colorsForLife = new Color[3];
    private int currentColorIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentColorIndex = 0;
        colorsForLife[0] = Color.yellow;
        colorsForLife[1] = new Vector4(1f, 0.5f, 0f, 0f);
        colorsForLife[2] = Color.red;

        playerHealth.playerRespawn += ChangeColor;
        
        playerHealth.playerHit += ChangeColor;
        StartCoroutine(OffSetVariation());
    }

    // Update is called once per frame
    void Update()
    {
         this.transform.position = Vector3.Lerp(transform.position, starPlaceHolder.position + offSet, movementForce);
    }

    IEnumerator OffSetVariation()
    {
        float startLimit = 0.5f;

        float speed = 0.5f;
        float direction = 1;
        
        while (true)
        {
            if (offSet.y >= startLimit) direction = -1;

            if (offSet.y <= -startLimit) direction = 1;

            offSet += Vector3.up * Time.deltaTime * direction * speed;
            

            yield return null;
        }

        
    }


    void ChangeColor()
    {

        
        switch (currentColorIndex)
        {
            
            case 2:
                currentColorIndex = 0;
                break;
            
            default:
                currentColorIndex++;
                break;
                
        }
        this.GetComponent<Renderer>().material.color = colorsForLife[currentColorIndex];
        // this.GetComponent<Renderer>().material.SetVector("_OutlineColor", colorsForLife[currentColorIndex]);
    }
    
    
}
