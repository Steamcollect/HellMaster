using UnityEngine;
public class Weapon_Glock : WeaponTemplate
{
    [Header("Settings")]
    [SerializeField] float maxShootDistance = 50;

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
    }

    public override bool CanAttack()
    {
        return true;
    }

    public override void Reload()
    {
        // Do nothing
    }
}