using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{
    [Header("Settings")]
    [SerializeField] float maxHealth;
    float currentHealth;
    [SerializeField] Animator animator;

    //[Header("References")]

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    [Header("Input")]
    [SerializeField] RSE_OnPlayerDeath rseOnPlayerDeath;

    //[Header("Output")]

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
        StartCoroutine(DeathAnim());
    }

    IEnumerator DeathAnim()
    {
        animator.SetTrigger("Death");
        Debug.Log("caca meurt");

        yield return new WaitForSeconds(0.767f);

        Destroy(gameObject);
    }
}