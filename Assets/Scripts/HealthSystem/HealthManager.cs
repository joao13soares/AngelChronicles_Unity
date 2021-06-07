using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{

    [SerializeField] private AudioClip collectSFX;
    
    [SerializeField] GameObject levelStartPostion;
    public Vector3 lastCheckPointPosition;

    public float feathersNeededForOneLife = 3;

    float lives, hitsRemaining;

    public float GetCurrentLives => lives;
    
    [SerializeField] float currentFeathers;
    public float GetCurrentFeathers => currentFeathers;

    
    
    // Invincibility Variables
    private bool playerInvincible;
    [SerializeField] private List<Renderer> renderersToFlash;
    [SerializeField] private const float invincibilityFramesTime = 1.5f;

    // EVENTS
    public delegate void PlayerEvent();
    public event PlayerEvent playerHit;
    public event PlayerEvent playerRespawned;
    public event PlayerEvent featherCollected, RemainingLivesChanged;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        lastCheckPointPosition = levelStartPostion.transform.position;
        lives = 3;
        hitsRemaining = 3;
        playerInvincible = false;
    }

    public void ChangeLastCheckpoint(Vector3 newCheckpointPos) => lastCheckPointPosition = newCheckpointPos;

    public void GetHit()
    {
        if (playerInvincible) return;
        
        

        if (hitsRemaining > 1)
        {
            hitsRemaining--;
            StartCoroutine(InvincibilityFrames());
            playerHit?.Invoke();
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
        playerInvincible = true;
        
        for (float i = 0; i <= invincibilityFramesTime; i += invincibilityFramesTime / 10f)
        {
            foreach (Renderer r in renderersToFlash) r.enabled = false;
            yield return new WaitForSeconds(invincibilityFramesTime/20f);
            foreach (Renderer r in renderersToFlash) r.enabled = true;
            yield return new WaitForSeconds(invincibilityFramesTime/20f);


        }

        playerInvincible = false;
        yield return null;


    }

    private void SpawnPlayerAtPosition(Vector3 positionToSpawn) => this.transform.position = positionToSpawn;

    private void Respawn()
    {
        SpawnPlayerAtPosition(lastCheckPointPosition);
        
        lives--;
        hitsRemaining = 3;
        
        
        playerRespawned?.Invoke();
        RemainingLivesChanged?.Invoke();
    }

    private void Restart()
    {
        // RestartLevel Logic
        SpawnPlayerAtPosition(levelStartPostion.transform.position);
        lives = 1;
    }


    public void FeatherCollected()
    {
        
        this.GetComponent<AudioSource>().PlayOneShot(collectSFX);
        
        currentFeathers++;
        featherCollected?.Invoke();

        if (currentFeathers != feathersNeededForOneLife) return;

        
        
        lives++;
        currentFeathers = 0;
        
        featherCollected?.Invoke();
        RemainingLivesChanged?.Invoke();
    }

    public void HaloCollected() => SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
}