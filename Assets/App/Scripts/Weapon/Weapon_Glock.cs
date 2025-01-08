using System.Collections;
using UnityEngine;
public class Weapon_Glock : WeaponTemplate
{
    [Header("Settings")]
    [SerializeField] float maxShootDistance = 50;

    [SerializeField] TrailRenderer bulletTrail;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] ParticleSystem shootingParticleSystem;
    [SerializeField] ParticleSystem impactParticleSystem;
    [SerializeField] ParticleSystem fleshParticleSystem;
    [SerializeField] Animator animator;

    //[Header("References")]

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    //[Header("Input")]
    //[Header("Output")]
    public override void Attack(Vector3 attackPosition, Vector3 attackDirection)
    {
        RaycastHit hit;

        shootingParticleSystem.Play();

        animator.SetTrigger("Attack");

        if (Physics.Raycast(attackPosition, attackDirection, out hit, maxShootDistance))
        {
            TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, hit));

            if (hit.transform.TryGetComponent(out IHealth health))
            {
                Instantiate(fleshParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
                health.TakeDamage(damage * damageMultiplier, OnTargetKill);
            }
            else
            {
                Instantiate(impactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }

    public override bool CanAttack()
    {
        return true;
    }

    public override void Reload()
    {
        // Do nothing
    }

    IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        trail.transform.position = hit.point;
        Instantiate(impactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(trail.gameObject, trail.time);
    }
}