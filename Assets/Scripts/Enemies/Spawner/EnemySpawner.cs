using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    
    [SerializeField]private GameObject enemyToSpawnPrefab;
    private GameObject currentEnemyAlive;


    private bool spawningEnemy;

    private Transform spawnerTransform;
    private void Awake()
    {
        spawnerTransform = transform;
        InstantiateEnemyPrefab();

        

    }

    private void SpawnEnemy()
    {
        if (spawningEnemy) return;

        StartCoroutine(SpawnEnemyCoroutine());


    }

    IEnumerator SpawnEnemyCoroutine()
    {
        spawningEnemy = true;
        
        yield return new WaitForSeconds(1.5f);

        InstantiateEnemyPrefab();

        spawningEnemy = false;

    }

    void InstantiateEnemyPrefab()
    {
        currentEnemyAlive = Instantiate(enemyToSpawnPrefab, spawnerTransform.position, spawnerTransform.rotation);
        currentEnemyAlive.GetComponent<Enemy>().EnemyDied += SpawnEnemy;
    }
    
}
