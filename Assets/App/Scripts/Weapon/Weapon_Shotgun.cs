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
    [SerializeField] RSE_CameraShake cameraShake;
    [SerializeField] float shakeRange;
    [SerializeField] float shakeTime;

    [SerializeField] RSE_Recoil rseRecoil;
    [SerializeField] float recoilRange;
    [SerializeField] float recoilTime;

    bool isReloading = false;

    [SerializeField] string bulletTrail;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] ParticleSystem shootingParticleSystem;
    [SerializeField] string impactParticleSystem;
    [SerializeField] string fleshParticleSystem;
    [SerializeField] LayerMask Mask;
    [SerializeField] Animator animator;
    [SerializeField] RSO_PoolManager rsoPoolManager;

    [Header("References")]
    [SerializeField] AudioClip[] reloadClips;

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

            shootingParticleSystem.Play();

            cameraShake.Call(shakeTime, shakeRange);

            animator.SetTrigger("Attack");

            if (Physics.Raycast(attackPosition, attackDirection, out hit, maxShootDistance))
            {
                TrailRenderer trail =
                    rsoPoolManager.Value.GetFromPool(bulletTrail, bulletSpawnPoint.position, Quaternion.identity).GetComponent<TrailRenderer>();

                StartCoroutine(SpawnTrail(trail, hit));

                if (hit.transform.TryGetComponent(out IHealth health))
                {
                    rsoPoolManager.Value.GetFromPool(fleshParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
                    health.TakeDamage(damage * damageMultiplier, OnTargetKill);
                }
                else
                {
                    rsoPoolManager.Value.GetFromPool(impactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }
        rseRecoil.Call(recoilRange, recoilTime);

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
            rsePlayClipAt.Call(reloadClips.GetRandom(), transform.position, 1);
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

        yield return new WaitForSeconds(trail.time);
        rsoPoolManager.Value.ReturnToPool("BulletTrail", trail.gameObject);
    }
}