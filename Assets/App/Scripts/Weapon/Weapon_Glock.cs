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

        if (Physics.Raycast(attackPosition, attackDirection, out hit, maxShootDistance))
        {
            TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, hit));

            if (hit.transform.TryGetComponent(out IHealth health))
            {
                health.TakeDamage(damage);
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
        //animator.SetBool("IsShooting", false);
        trail.transform.position = hit.point;
        Instantiate(impactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(trail.gameObject, trail.time);
    }
}