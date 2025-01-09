using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [Header("Settings")]
    [SerializeField] float maxHealth;
    float currentHealth;

    float timeAlive;

    [Header("References")]

    //[Space(10)]
    // RSO
    [SerializeField] RSO_ContentSaved rsoContentSaved;
    // RSF
    // RSP

    [Header("Achievment")]
    [SerializeField] SSO_Achievment_SurvivMinTime[] achievmentsSurvivMinTime;

    [Header("Input")]
    [SerializeField] RSE_AddPlayerMaxHealth rseAddMaxHealth;
    [SerializeField] RSE_OnGameStart rseOnGameStart;
    [SerializeField] RSE_OnPlayerDeath rseOnPlayerDeath;

    [Header("Output")]
    [SerializeField] RSE_UdateHealthBar rseUpdateHealthBar;

    void OnEnable()
    {
        rseAddMaxHealth.action += TakeMaxHealth;
        rseOnGameStart.action += OnGameStart;
        rseOnPlayerDeath.action += OnPlayerDeath;
    }
    void OnDisable()
    {
        rseAddMaxHealth.action -= TakeMaxHealth;
        rseOnGameStart.action -= OnGameStart;
        rseOnPlayerDeath.action -= OnPlayerDeath;
    }

    void Update()
    {
        timeAlive += Time.deltaTime;
    }

    void OnGameStart()
    {
        timeAlive = rsoContentSaved.Value.totalTimeAlive;

        currentHealth = maxHealth;
        rseUpdateHealthBar.Call(currentHealth, maxHealth);

        foreach (var item in achievmentsSurvivMinTime)
        {
            item.delayTimer = StartCoroutine(item.Delay());
        }
    }
    void OnPlayerDeath()
    {
        rsoContentSaved.Value.totalTimeAlive = timeAlive;
    }

    public void TakeMaxHealth(int health)
    {
        maxHealth += health;
        currentHealth += health;

        rseUpdateHealthBar.Call(currentHealth, maxHealth);
    }

    public void TakeDamage(float damage, Action onDeath)
    {
        currentHealth -= damage;
        if(currentHealth < 0) currentHealth = 0;
        rseUpdateHealthBar.Call(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            onDeath?.Invoke();
            Die();
        }
    }

    void Die()
    {
        foreach (var item in achievmentsSurvivMinTime)
        {
            StopCoroutine(item.delayTimer);
        }

        rseOnPlayerDeath.Call();
        print("Player is dead");
    }
}