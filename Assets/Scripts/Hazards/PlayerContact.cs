using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContact : MonoBehaviour
{
   void OnCollisionStay(Collision other)
   {
      HealthManager playerHp = other.gameObject.GetComponent<HealthManager>();

      
      
      Debug.Log(other.gameObject);
      if (playerHp != null)
         DamagePlayer(playerHp);
      
      
      
   }

   public void DamagePlayer(HealthManager playerHp) => playerHp.GetHit();


}
