using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_RocketLauncher : WeaponTemplate
{
    [Header("Settings")]

    [Space(10)]
    [SerializeField] float reloadTime;
    [SerializeField] int maxBulletCount;
    [SerializeField] int bulletCount;

    [SerializeField] RSE_Recoil rseRecoil;
    [SerializeField] float recoilRange;
    [SerializeField] float recoilTime;

    bool isReloading = false;

    [SerializeField] Weapon_Missile missile;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] ParticleSystem shootingParticleSystem;
    [SerializeField] Animator animator;

    public override void Attack(Vector3 attackPosition, Vector3 attackDirection)
    {
        shootingParticleSystem.Play();

        rseRecoil.Call(recoilRange, recoilTime);

        animator.SetTrigger("Attack");

        Weapon_Missile tempMissile = Instantiate(missile, bulletSpawnPoint.transform.position, Quaternion.LookRotation(attackDirection));
        tempMissile.damage = damage;
        tempMissile.damageMultiplier = damageMultiplier;
        tempMissile.OnTargetKill += OnTargetKill;
        tempMissile.forward = attackDirection;

        bulletCount--;
        if (bulletCount <= 0)
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
}
