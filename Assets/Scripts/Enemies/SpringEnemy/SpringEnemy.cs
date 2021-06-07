using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringEnemy : Enemy
{
   void Awake()
   {
      canBeGrabbed = true;
      grabAction = new NormalGrab();
      consumableAction = new SpawnPrefabConsume();
      throwAction = new NormalThrow();
      
   }


   void PlayBoingSoundEffect() => this.GetComponent<AudioSource>().Play();
}
