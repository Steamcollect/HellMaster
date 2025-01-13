using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Sword : WeaponTemplate
{
    [Header("Settings")]
    [SerializeField] bool isAttacking;
    [SerializeField] float duration;
    [SerializeField] Animator animator;

    [SerializeField] Vector3 raycastSize;
    [SerializeField] Vector3 raycastPosOffset;

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

        // Perform a box cast in front of the weapon
        Vector3 raycastPosition = transform.position + transform.forward * raycastPosOffset.z + transform.up * raycastPosOffset.y + transform.right * raycastPosOffset.x;
        Collider[] hitColliders = Physics.OverlapBox(raycastPosition, raycastSize, transform.rotation);

        foreach (var collider in hitColliders)
        {
            if (collider.TryGetComponent(out IHealth health))
            {
                health.TakeDamage(damage * damageMultiplier, OnTargetKill);
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
        Vector3 raycastPosition = transform.position + transform.forward * raycastPosOffset.z + transform.up * raycastPosOffset.y + transform.right * raycastPosOffset.x;
        Gizmos.matrix = Matrix4x4.TRS(raycastPosition, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, raycastSize);
    }
}
