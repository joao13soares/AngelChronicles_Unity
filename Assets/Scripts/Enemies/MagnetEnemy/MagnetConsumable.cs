using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetConsumable : MonoBehaviour
{
    GameObject player;
    [SerializeField] private AudioClip magnetConsumeSFX;

    void Start()
    {
        player = GameObject.Find("Player");
        
        this.GetComponent<AudioSource>().PlayOneShot(magnetConsumeSFX);
         // player.GetComponent<PlayerMovement>().ChangeGravityDirection();
         player.GetComponent<PlayerMovement>().isMagnetized = !player.GetComponent<PlayerMovement>().isMagnetized;
         player.GetComponent<PlayerMovement>().jumping = false;
        //Physics.gravity = -Physics.gravity;
        StartCoroutine(WaitForDestroy());
        
    }

    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(magnetConsumeSFX.length);
        Destroy(this.gameObject);



    }
    
}
