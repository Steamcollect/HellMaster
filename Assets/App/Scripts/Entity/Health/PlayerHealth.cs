using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [Header("Settings")]
    [SerializeField] int maxHealth;
    int currentHealth;

    //[Header("References")]

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    //[Header("Input")]
    //[Header("Output")]

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeHealth(int health)
    {
        currentHealth += health;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Action onDeath)
    {
        currentHealth -= damage;
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