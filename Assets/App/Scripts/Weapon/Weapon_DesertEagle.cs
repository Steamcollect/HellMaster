using UnityEngine;
using System.Collections;

public class Weapon_DesertEagle : WeaponTemplate
{
    [Header("Settings")]
    [SerializeField] float maxShootDistance = 50;

    [Space(10)]
    [SerializeField] float reloadTime;
    [SerializeField] int maxBulletCount;
    [SerializeField] int bulletCount;
    bool isReloading = false;

    [SerializeField] TrailRenderer bulletTrail;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] ParticleSystem shootingParticleSystem;
    [SerializeField] ParticleSystem impactParticleSystem;
    [SerializeField] LayerMask Mask;
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

        //animator.SetBool("IsShooting", true);
        shootingParticleSystem.Play();

        if (Physics.Raycast(attackPosition, attackDirection, out hit, maxShootDistance))
        {
            TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, hit));

            if (hit.transform.TryGetComponent(out IHealth health))
            {
                health.TakeDamage(damage);
            }
        }

        bulletCount--;
    }

    public override bool CanAttack()
    {
        return !isReloading && bulletCount > 0;
    }

    public override void Reload()
    {
        StartCoroutine(ReloadDelay());
    }

    IEnumerator ReloadDelay()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);

        bulletCount = maxBulletCount;
        isReloading = false;
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