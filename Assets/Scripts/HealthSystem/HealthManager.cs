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


    // Start is called before the first frame update
    void Awake()
    {
        lastCheckPointPosition = levelStartPostion;
        lives = 3;
        hitsRemaining = 3;
    }

    public void ChangeLastCheckpoint(Vector3 newCheckpointPos) => lastCheckPointPosition = newCheckpointPos;

    public void GetHit()
    {
        if (hitsRemaining > 0)
        {
            hitsRemaining--;
            return;
        }

        else if (lives > 0)
        {
            Respawn();
            return;
        }

        else Restart();

    }

    private void SpawnPlayerAtPosition(Vector3 positionToSpawn) => this.transform.position = positionToSpawn;

    private void Respawn()
    {
        SpawnPlayerAtPosition(lastCheckPointPosition);
        lives--;
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
