using System;
using System.Collections.Generic;
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
    [SerializeField] AudioClip[] takeDamageSounds;
    [SerializeField] AudioClip[] healSounds;
    [SerializeField] AudioClip[] deathSounds;
    //[Space(10)]
    // RSO
    [SerializeField] RSO_ContentSaved rsoContentSaved;
    // RSF
    // RSP

    [Header("Achievment")]
    List<SSO_Achievment_SurvivMinTime> achievmentsSurvivMinTime = new();
    List<SSO_Achivment_HealCount> achivmentHealCount = new();
    [SerializeField] SSO_Achivment_CompleteOnce onDeathAchivment;

    [Space(10)]
    [SerializeField] RSO_Achievements rsoAchievements;

    [Header("Input")]
    [SerializeField] RSE_AddPlayerMaxHealth rseAddMaxHealth;
    [SerializeField] RSE_OnGameStart rseOnGameStart;
    [SerializeField] RSE_OnPlayerDeath rseOnPlayerDeath;
    [SerializeField] RSE_SaveAllGameData rseSaveGameData;

    [Header("Output")]
    [SerializeField] RSE_UdateHealthBar rseUpdateHealthBar;
    [SerializeField] RSE_CameraShake rseCameraShake;
    [SerializeField] RSE_PlayClipAt rsePlayClipAt;
    [SerializeField] RSE_OnHealth rseOnHealth;
    [SerializeField] RSE_OnPlayerHit rseOnPlayerHit;

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
        foreach (var achievement in rsoAchievements.Value)
        {
            if (achievement is SSO_Achievment_SurvivMinTime achievementSurviv)
            {
                achievmentsSurvivMinTime.Add(achievementSurviv);
            }
            if(achievement is SSO_Achivment_HealCount achivmentHeal)
            {
                achivmentHealCount.Add(achivmentHeal);
            }
        }


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
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        rseOnHealth.Call(currentHealth);
        rseUpdateHealthBar.Call(currentHealth, maxHealth);

        rsePlayClipAt.Call(healSounds.GetRandom(), transform.position, 1);

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
        rsePlayClipAt.Call(takeDamageSounds.GetRandom(), transform.position, 1);
        rseOnPlayerHit.Call();

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

        rsePlayClipAt.Call(deathSounds.GetRandom(), transform.position, 1);
        rseOnPlayerDeath.Call();
        onDeathAchivment.Achieve();
        print("Player is dead");
    }
}