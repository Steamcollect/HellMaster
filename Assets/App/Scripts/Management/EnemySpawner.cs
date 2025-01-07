using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] List<EnemySpawn> enemys = new();

    [Header("References")]
    [SerializeField] Transform[] spawnPoints;

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    //[Header("Input")]
    //[Header("Output")]

    private void Start()
    {
        foreach (var enemy in enemys)
        {
            SpawnEnemy(enemy);
        }
    }

    void SpawnEnemy(EnemySpawn enemy)
    {
        Instantiate(enemy.enemyPrefab, spawnPoints.GetRandom().position, Quaternion.identity);
        StartCoroutine(EnemySpawnDelay(enemy, SpawnEnemy));
    }

    IEnumerator EnemySpawnDelay(EnemySpawn enemy, Action<EnemySpawn> action)
    {
        yield return new WaitForSeconds(enemy.enemySpawnDelay);

        action.Invoke(enemy);
    }
}

[System.Serializable]
public class EnemySpawn
{
    public GameObject enemyPrefab;
    public float enemySpawnDelay;
}