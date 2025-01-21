using System;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
public class Barrel : MonoBehaviour, IHealth
{
    [Header("Settings")]
    float currentHealth;
    [SerializeField] float maxHealth;
    [SerializeField] RSE_CameraShake cameraShake;
    [SerializeField] Weapon_Explosion explosion;
    [SerializeField] float shakeRange;
    [SerializeField] float shakeTime;
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

    public void Heal(int health)
    {
        
    }

    public void TakeDamage(float damage, Action onDeath)
    {
        currentHealth -= damage;

        if(currentHealth < 0)
        {
            cameraShake.Call(shakeTime, shakeRange);
            Weapon_Explosion tempExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
            tempExplosion.OnTargetKill += onDeath;
            tempExplosion.damage = damage;
            tempExplosion.damageMultiplier = 1;

            Destroy(gameObject);
        }
    }

    public void TakeMaxHealth(int health)
    {
        
    }
}