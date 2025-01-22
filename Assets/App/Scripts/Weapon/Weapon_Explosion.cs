using System;
using System.Collections;
using UnityEngine;
public class Weapon_Explosion : MonoBehaviour
{
    public float damage;
    public float damageMultiplier;
    public Action OnTargetKill;

    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] AudioClip[] explosionsClips;
    [SerializeField] RSE_PlayClipAt rsePlayClipAt;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IHealth health) && !other.CompareTag("Player"))
        {
            health.TakeDamage(damage * damageMultiplier, OnTargetKill);
        }
        StartCoroutine("ExplosionDelay");
    }

    IEnumerator ExplosionDelay()
    {
        rsePlayClipAt.Call(explosionsClips.GetRandom(), transform.position, 1);

        yield return new WaitForSeconds(0.75f);
        DetachParticles();
        Destroy(gameObject);
    }

    public void DetachParticles()
    {
        // This splits the particle off so it doesn't get deleted with the parent
        explosionParticles.transform.parent = null;
    }
}