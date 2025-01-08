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

    //[Header("Input")]
    [Header("Output")]
    [SerializeField] RSE_UdateHealthBar rseUpdateHealthBar;

    void Start()
    {
        currentHealth = maxHealth;
        rseUpdateHealthBar.Call(currentHealth, maxHealth);
    }

    public void TakeHealth(int health)
    {
        currentHealth += health;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

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
        print("Player is dead");
    }
}