using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth, IResettable
{
    [Header("Settings")]
    [SerializeField] float maxHealth;
    float currentHealth;
    [SerializeField] EnemyController controller;
    [SerializeField] Animator animator;

    [SerializeField] int scoreGiven;

    [Space(10)]
    [SerializeField] string poolName;
    [SerializeField] RSO_PoolManager rsoPoolManager;

    //[Header("References")]

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    [Header("Input")]
    [SerializeField] RSE_OnPlayerDeath rseOnPlayerDeath;

    [Header("Output")]
    [SerializeField] RSE_AddScore rseAddScore;
    [SerializeField] RSE_OnEnemyHit rseOnEnemyHit;

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
        ResetState();
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
    public void Heal(int health)
    {
        currentHealth += health;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }
    public void TakeDamage(float damage, Action onDeath)
    {
        rseOnEnemyHit.Call(transform.position);

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
        rseAddScore.Call(scoreGiven);
        StartCoroutine(DeathAnim());
    }

    IEnumerator DeathAnim()
    {
        animator.SetTrigger("Death");

        yield return new WaitForSeconds(1f);

        rsoPoolManager.Value.ReturnToPool(poolName, gameObject);
    }

    public void ResetState()
    {
        currentHealth = maxHealth;
    }
}