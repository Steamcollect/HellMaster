using System;
using System.Collections;
using UnityEngine;

public abstract class WeaponTemplate : MonoBehaviour
{
    [Header("Settings")]
    public float damage;
    public float damageMultiplier = 1;

    [Space(5)]
    public float attackDelay;
    public float attackRateMultiplier = 1;
    public bool isSemiAuto;
    public bool hasSpread;
    public Vector3 bulletSpreadVariance;

    bool canAttack = true;

    public Action OnTargetKill;

    [Header("References")]
    [SerializeField] GameObject visual;

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    //[Header("Input")]
    //[Header("Output")]

    public void OnAttack(Vector3 attackPosition, Vector3 attackDirection)
    {
        if (canAttack && CanAttack())
        {
            if(hasSpread)
            {
                attackDirection += new Vector3(
                    UnityEngine.Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
                    UnityEngine.Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y), 
                    UnityEngine.Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z));
            }

            Attack(attackPosition, attackDirection);
            StartCoroutine(AttackDelay());
        }
    }
    public abstract void Attack(Vector3 attackPosition, Vector3 attackDirection);

    public abstract bool CanAttack();

    public abstract void Reload();

    IEnumerator AttackDelay()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackDelay * attackRateMultiplier);
        canAttack = true;
    }

    public void Hide()
    {
        visual.SetActive(false);
    }
    public void Show()
    {
        visual.SetActive(true);
    }
}