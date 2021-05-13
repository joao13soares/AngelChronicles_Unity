using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    
    [SerializeField]private GameObject enemyToSpawnPrefab;
    private GameObject currentEnemyAlive;

    private Transform spawnerTransform;
    private void Awake()
    {
        spawnerTransform = transform;
        SpawnEnemy();


    }

    private void SpawnEnemy()
    {
        
        currentEnemyAlive = Instantiate(enemyToSpawnPrefab, spawnerTransform.position, spawnerTransform.rotation);
        currentEnemyAlive.GetComponent<Enemy>().EnemyDied += SpawnEnemy;

    }
}
