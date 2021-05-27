using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetConsumable : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().isMagnetized = !player.GetComponent<PlayerMovement>().isMagnetized;
        player.GetComponent<PlayerMovement>().jumping = false;
        //Physics.gravity = -Physics.gravity;
        GameObject.DestroyImmediate(this.gameObject);
    }
}
