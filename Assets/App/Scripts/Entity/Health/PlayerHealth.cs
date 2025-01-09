using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [Header("Settings")]
    [SerializeField] float maxHealth;
    float currentHealth;

    //[Header("References")]

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    [Header("Achievment")]
    [SerializeField] SSO_Achievment_SurvivMinTime[] achievmentsSurvivMinTime;

    [Header("Input")]
    [SerializeField] RSE_AddPlayerMaxHealth rseAddMaxHealth;
    
    [Header("Output")]
    [SerializeField] RSE_UdateHealthBar rseUpdateHealthBar;

    void OnEnable()
    {
        rseAddMaxHealth.action += TakeMaxHealth;
    }
    void OnDisable()
    {
        rseAddMaxHealth.action -= TakeMaxHealth;
    }

    void Start()
    {
        currentHealth = maxHealth;
        rseUpdateHealthBar.Call(currentHealth, maxHealth);

        foreach (var item in achievmentsSurvivMinTime)
        {
            item.delayTimer = StartCoroutine(item.Delay());
        }
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
        print("Player is dead");
    }
}