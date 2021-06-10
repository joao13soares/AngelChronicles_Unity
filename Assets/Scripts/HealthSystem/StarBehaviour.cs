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
        
        // Sets up colors
        currentColorIndex = 0;
        colorsForLife[0] = Color.yellow;
        colorsForLife[1] = new Vector4(1f, 0.5f, 0f, 0f);
        colorsForLife[2] = Color.red;

        // Subscribe to needed events
        playerHealth.playerRespawned += ChangeColor;
        playerHealth.playerHit += ChangeColor;
        
        
        StartCoroutine(OffSetVariation());
    }


    IEnumerator OffSetVariation()
    {
        float startLimit = 0.5f;

        float speed = 0.5f;
        float direction = 1;
        
        while (true)
        {
            // Up and Down
            if (offSet.y >= startLimit) direction = -1;
            if (offSet.y <= -startLimit) direction = 1;

            offSet += Vector3.up * Time.deltaTime * direction * speed;
            
            
            // Follow target position
            this.transform.position = Vector3.Lerp(transform.position, starPlaceHolder.position + offSet, movementForce);


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
    }
    
    
}
