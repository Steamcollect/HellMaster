using System;
using System.Collections;
using UnityEngine;

public class FlyingEnemyHealth : MonoBehaviour, IHealth, IResettable
{
    //Désolé si c'est dégueulasse et que ça fait du copié-collé de code, mais c'est moi le GD qui ait fait ça rapidement pour que ça marche :,)

    [Header("Settings")]
    [SerializeField] float maxHealth;
    float currentHealth;
    [SerializeField] FlyingEnemyController controller;
    [SerializeField] Animator animator;
    [SerializeField] int scoreGiven;

    [Space(10)]
    [SerializeField] string poolName;
    [SerializeField] RSO_PoolManager rsoPoolManager;

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
        ResetObj();
    }

    public void ResetState()
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

    public void ResetObj()
    {
        throw new NotImplementedException();
    }
}