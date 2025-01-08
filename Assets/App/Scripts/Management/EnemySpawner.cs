using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] List<EnemySpawn> enemys = new();

    float gameTime;

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

    private void Update()
    {
        gameTime += Time.deltaTime;
    }

    void SpawnEnemy(EnemySpawn enemy)
    {
        if (enemy.enemySpawnDelay[0].time > gameTime)
        {
            StartCoroutine(EnemySpawnDelay(enemy, SpawnEnemy, enemy.enemySpawnDelay[0].time - gameTime));
        }
        else
        {
            Instantiate(enemy.enemyPrefab, spawnPoints.GetRandom().position, Quaternion.identity);
            StartCoroutine(EnemySpawnDelay(enemy, SpawnEnemy));
        }
    }

    IEnumerator EnemySpawnDelay(EnemySpawn enemy, Action<EnemySpawn> action)
    {
        yield return new WaitForSeconds(enemy.enemySpawnDelay.Evaluate(gameTime));

        action.Invoke(enemy);
    }
    IEnumerator EnemySpawnDelay(EnemySpawn enemy, Action<EnemySpawn> action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke(enemy);
    }
}

[System.Serializable]
public class EnemySpawn
{
    public GameObject enemyPrefab;
    public AnimationCurve enemySpawnDelay;
}