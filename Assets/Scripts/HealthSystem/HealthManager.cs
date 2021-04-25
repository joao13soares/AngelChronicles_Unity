using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] Vector3 levelStartPostion;
    Vector3 lastCheckPointPosition;

    const float featherNeededForOneLife = 10;

    float lives, hitsRemaining;
    [SerializeField] float currentFeathers;

    private bool playerInvincible;
    [SerializeField] private const float invincibilityFramesTime = 1.5f;

    
    //TESTES
    private Vector3 level1Start = new Vector3(-51, 4, 30);

    // Start is called before the first frame update
    void Awake()
    {
        //lastCheckPointPosition = levelStartPostion;
        lastCheckPointPosition = level1Start;
        lives = 3;
        hitsRemaining = 3;
        playerInvincible = false;
    }

    public void ChangeLastCheckpoint(Vector3 newCheckpointPos) => lastCheckPointPosition = newCheckpointPos;

    public void GetHit()
    {
        if (playerInvincible) return;

        if (hitsRemaining > 0)
        {
            hitsRemaining--;
            StartCoroutine(InvincibilityFrames());
            return;
        }

        if (lives > 0)
        {
            Respawn();
            return;
        }

        Restart();
    }

    IEnumerator InvincibilityFrames()
    {

        Renderer temp = this.GetComponent<Renderer>();
        
        playerInvincible = true;
        
        temp.enabled = false;
        
        yield return new WaitForSeconds(invincibilityFramesTime);
        
        temp.enabled = true;
        
        playerInvincible = false;
        
        
    }

    private void SpawnPlayerAtPosition(Vector3 positionToSpawn) => this.transform.position = positionToSpawn;

    private void Respawn()
    {
        SpawnPlayerAtPosition(lastCheckPointPosition);
        lives--;
        hitsRemaining = 3;
    }

    private void Restart()
    {
        // RestartLevel Logic
        SpawnPlayerAtPosition(levelStartPostion);
        lives = 1;
    }


    public void FeatherCollected()
    {
        currentFeathers++;

        if (currentFeathers != featherNeededForOneLife) return;

        lives++;
        currentFeathers = 0;
    }

    public void HaloCollected() => lives++;
}