using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, Health
{
    [SerializeField] int maxHealth;
    int currentHealth;

    public void TakeHealth(int health)
    {
        currentHealth += health;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        print("Player is dead");
    }
}