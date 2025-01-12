using System;
using System.Collections;
using UnityEngine;

public class FlyingEnemyHealth : MonoBehaviour, IHealth
{
    //D�sol� si c'est d�gueulasse et que �a fait du copi�-coll� de code, mais c'est moi le GD qui ait fait �a rapidement pour que �a marche :,)

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