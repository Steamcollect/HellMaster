using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] List<EnemySpawn> enemys = new();

    float gameTime;
    bool canSpawn = false;

    [Header("References")]
    [SerializeField] Transform[] spawnPoints;

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    [Header("Input")]
    [SerializeField] RSE_OnGameStart rseOnGameStart;
    [SerializeField] RSE_OnPlayerDeath rseOnPlayerDeath;
    //[Header("Output")]

    private void OnEnable()
    {
        rseOnGameStart.action += OnGameStart;
        rseOnPlayerDeath.action += OnPlayerDeath;
    }
    private void OnDisable()
    {
        rseOnGameStart.action -= OnGameStart;
        rseOnPlayerDeath.action -= OnPlayerDeath;
    }

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

    void OnGameStart()
    {
        canSpawn = true;
    }
    void OnPlayerDeath()
    {
        canSpawn = false;
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
        if (!canSpawn) yield return new WaitUntil(() => canSpawn);

        yield return new WaitForSeconds(enemy.enemySpawnDelay.Evaluate(gameTime));

        action.Invoke(enemy);
    }
    IEnumerator EnemySpawnDelay(EnemySpawn enemy, Action<EnemySpawn> action, float delay)
    {
        if (!canSpawn) yield return new WaitUntil(() => canSpawn);

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