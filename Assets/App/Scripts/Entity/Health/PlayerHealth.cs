using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [Header("Settings")]
    [SerializeField] float maxHealth;
    float currentHealth;

    float timeAlive;

    [Space(10)]
    [SerializeField] float hitShakeRange;
    [SerializeField] float hitShakeTime;

    [Header("References")]

    //[Space(10)]
    // RSO
    [SerializeField] RSO_ContentSaved rsoContentSaved;
    // RSF
    // RSP

    [Header("Achievment")]
    [SerializeField] SSO_Achievment_SurvivMinTime[] achievmentsSurvivMinTime;
    [SerializeField] SSO_Achivment_HealCount[] achivmentHealCount;
    [SerializeField] SSO_Achivment_CompleteOnce onDeathAchivment;

    [Header("Input")]
    [SerializeField] RSE_AddPlayerMaxHealth rseAddMaxHealth;
    [SerializeField] RSE_OnGameStart rseOnGameStart;
    [SerializeField] RSE_OnPlayerDeath rseOnPlayerDeath;
    [SerializeField] RSE_SaveAllGameData rseSaveGameData;

    [Header("Output")]
    [SerializeField] RSE_UdateHealthBar rseUpdateHealthBar;
    [SerializeField] RSE_CameraShake rseCameraShake;

    void OnEnable()
    {
        rseAddMaxHealth.action += TakeMaxHealth;
        rseOnGameStart.action += OnGameStart;
        rseOnPlayerDeath.action += OnPlayerDeath;
        rseSaveGameData.action += SaveGameData;
    }
    void OnDisable()
    {
        rseAddMaxHealth.action -= TakeMaxHealth;
        rseOnGameStart.action -= OnGameStart;
        rseOnPlayerDeath.action -= OnPlayerDeath;
        rseSaveGameData.action -= SaveGameData;
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
    void SaveGameData()
    {
        rsoContentSaved.Value.totalTimeAlive = timeAlive;
    }
    public void TakeMaxHealth(int health)
    {
        maxHealth += health;
        currentHealth += health;

        rseUpdateHealthBar.Call(currentHealth, maxHealth);
    }
    public void Heal(int health)
    {
        currentHealth += health;
        if(currentHealth > maxHealth) currentHealth = maxHealth;
        rseUpdateHealthBar.Call(currentHealth, maxHealth);

        rsoContentSaved.Value.healCount++;
        foreach (var item in achivmentHealCount)
        {
            item.CheckHealCount(rsoContentSaved.Value.healCount);
        }
    }
    public void TakeDamage(float damage, Action onDeath)
    {
        currentHealth -= damage;
        rseCameraShake.Call(hitShakeTime, hitShakeRange);

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
            if(item.delayTimer != null) StopCoroutine(item.delayTimer);
        }

        rseOnPlayerDeath.Call();
        onDeathAchivment.Achieve();
        print("Player is dead");
    }
}