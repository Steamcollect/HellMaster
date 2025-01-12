using System;
using System.Collections;
using UnityEngine;

public class FlyingEnemyHealth : MonoBehaviour, IHealth
{
    //Désolé si c'est dégueulasse et que ça fait du copié-collé de code, mais c'est moi le GD qui ait fait ça rapidement pour que ça marche :,)

    [Header("Settings")]
    [SerializeField] float maxHealth;
    float currentHealth;
    [SerializeField] FlyingEnemyController controller;
    [SerializeField] Animator animator;

    [Header("Input")]
    [SerializeField] RSE_OnPlayerDeath rseOnPlayerDeath;

    void OnEnable()
    {
        rseOnPlayerDeath.action += OnPlayerDeath;
    }
    void OnDisable()
    {
        rseOnPlayerDeath.action -= OnPlayerDeath;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void OnPlayerDeath()
    {
        Destroy(gameObject);
    }

    public void TakeMaxHealth(int health)
    {
        maxHealth += health;
        currentHealth += health;
    }

    public void TakeDamage(float damage, Action onDeath)
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
        controller.isDead = true;
        StartCoroutine(DeathAnim());
    }

    IEnumerator DeathAnim()
    {
        animator.SetTrigger("Death");

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}