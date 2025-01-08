using UnityEngine;
using System.Collections;

public class Weapon_Shotgun : WeaponTemplate
{
    [Header("Settings")]
    [SerializeField] float maxShootDistance = 50;

    [Space(10)]
    [SerializeField] float reloadTime;
    [SerializeField] int bulletShotForOneShot;
    [SerializeField] int maxBulletCount;
    [SerializeField] int bulletCount;
    bool isReloading = false;

    [SerializeField] TrailRenderer bulletTrail;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] ParticleSystem shootingParticleSystem;
    [SerializeField] ParticleSystem impactParticleSystem;
    [SerializeField] ParticleSystem fleshParticleSystem;
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
        for (int i = 0; i < bulletShotForOneShot; i++)
        {
            attackDirection += new Vector3(
                    Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
                    Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y),
                    Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z));

            RaycastHit hit;

            //animator.SetBool("IsShooting", true);
            shootingParticleSystem.Play();

            if (Physics.Raycast(attackPosition, attackDirection, out hit, maxShootDistance))
            {
                TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hit));

                if (hit.transform.TryGetComponent(out IHealth health))
                {
                    Instantiate(fleshParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
                    health.TakeDamage(damage, OnTargetKill);
                }
                else
                {
                    Instantiate(impactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }

        bulletCount--;
        if(bulletCount <= 0)
        {
            Reload();
        }
    }

    public override bool CanAttack()
    {
        return !isReloading && bulletCount > 0;
    }

    public override void Reload()
    {
        if (!isReloading)
        {
            animator.SetTrigger("Reload");
            StartCoroutine(ReloadDelay());
        }
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

        Destroy(trail.gameObject, trail.time);
    }
}