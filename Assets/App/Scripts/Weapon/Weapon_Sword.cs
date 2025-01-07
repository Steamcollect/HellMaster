using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Sword : WeaponTemplate
{
    [Header("Settings")]
    [SerializeField] float attackDistance = 2;
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

        RaycastHit[] hits = Physics.RaycastAll(attackPosition, attackDirection, attackDistance);

        animator.SetTrigger("Attack");

        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.TryGetComponent(out IHealth health))
                {
                    health.TakeDamage(damage);
                }
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * attackDistance);
    }
}
