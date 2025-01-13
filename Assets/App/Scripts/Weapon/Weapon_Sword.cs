using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Sword : WeaponTemplate
{
    [Header("Settings")]
    [SerializeField] bool isAttacking;
    [SerializeField] float duration;
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
        transform.forward = attackDirection;

        animator.SetTrigger("Attack");

        StartCoroutine("SwipeDelay");
    }

    public override bool CanAttack()
    {
        return true;
    }

    public override void Reload()
    {
        // Do nothing
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position, transform.position + transform.forward * attackDistance);
    }

    private void OnTriggerStay(Collider other)
    {
        if (isAttacking)
        {
            if (other.TryGetComponent(out IHealth health))
            {
                health.TakeDamage(damage * damageMultiplier, OnTargetKill);
            }
        }
    }

    IEnumerator SwipeDelay()
    {
        isAttacking = true;
        yield return new WaitForSeconds(duration);
        isAttacking = false;
    }
}
