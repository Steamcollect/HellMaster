using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
public class PlayerCombat : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int damage;
    [SerializeField] float maxShootDistance;

    [Space(5)]
    [SerializeField] float attackCooldown;
    bool canAttack = true;

    [Space(10)]
    [SerializeField] float reloadTime;
    [SerializeField] int maxBulletCount;
    int bulletCount;
    bool isReloading = false;

    [Header("References")]
    [SerializeField] Transform cam;

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    //[Header("Input")]
    //[Header("Output")]

    private void Start()
    {
        bulletCount = maxBulletCount;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && canAttack && !isReloading)
        {
            if(bulletCount <= 0)
            {
                StartCoroutine(Reload());
            }
            else
            {
                Attack();
                StartCoroutine(AttackCooldown());
            }
        }

        if (Input.GetKey(KeyCode.R) && !isReloading && bulletCount < maxBulletCount)
        {
            StartCoroutine(Reload());
        }
    }

    void Attack()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, maxShootDistance))
        {
            if(hit.transform.TryGetComponent(out IHealth health))
            {
                health.TakeDamage(damage);
            }
        }

        bulletCount--;
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);

        bulletCount = maxBulletCount;
        isReloading = false;
    }
}