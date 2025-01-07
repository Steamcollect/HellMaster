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
}