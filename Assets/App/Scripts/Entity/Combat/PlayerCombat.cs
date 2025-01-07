using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] WeaponTemplate weapon;
    [SerializeField] Transform cam;

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    //[Header("Input")]
    //[Header("Output")]

    private void Start()
    {
        weapon.Reload();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && weapon.isSemiAuto || Input.GetKeyDown(KeyCode.Mouse0))
        {
            weapon.OnAttack(cam.position, cam.forward);
        }

        if (Input.GetKey(KeyCode.R))
        {
            weapon.Reload();
        }
    }
}