using System;
using System.Collections;
using UnityEngine;
public class Weapon_Explosion : MonoBehaviour
{
    public float damage;
    public float damageMultiplier;
    public Action OnTargetKill;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IHealth health))
        {
            health.TakeDamage(damage * damageMultiplier, OnTargetKill);
        }
        StartCoroutine("ExplosionDelay");
    }

    IEnumerator ExplosionDelay()
    {
        yield return new WaitForSeconds(0.75f);
        Destroy(gameObject);
    }
}