using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectiblesUI : MonoBehaviour
{
    [SerializeField] private HealthManager player;
    
    [SerializeField] private Text maxFeathersText; 
    [SerializeField] private Text currentFeathersText; 
    [SerializeField] private Text currentLifesText; 


    
    
    // Start is called before the first frame update
    void Start()
    {
        maxFeathersText.text ="/" + player.feathersNeededForOneLife;
        currentFeathersText.text = "0";
        currentLifesText.text = "Lives: x" + player.GetCurrentLives;

        player.featherCollected += ChangeCurrentFeathers;
        player.RemainingLivesChanged += ChangeCurrentLives;

    }

    void ChangeCurrentFeathers() => currentFeathersText.text = player.GetCurrentFeathers.ToString();

    void ChangeCurrentLives() => currentLifesText.text ="Lives: x" + player.GetCurrentLives;
    
}
