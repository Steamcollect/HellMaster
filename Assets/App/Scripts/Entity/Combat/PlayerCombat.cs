using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform weaponsContent;

    [SerializeField] List<WeaponTemplate> weapons = new();
    [SerializeField] int currentWeaponIndex = 0;
    [SerializeField] Transform cam;

    float damageMultiplier;

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    [Header("Input")]
    [SerializeField] RSE_AddDamageMultiplier rseAddDamageMult;

    //[Header("Output")]

    private void OnEnable()
    {
        rseAddDamageMult.action += AddDamageMultiplier;
    }
    private void OnDisable()
    {
        rseAddDamageMult.action -= AddDamageMultiplier;
    }

    private void Start()
    {
        weapons[currentWeaponIndex].damageMultiplier = damageMultiplier;
        weapons[currentWeaponIndex].OnTargetKill += OnEnemyKill;
    }

    private void Update()
    {
        int newWeaponIndex = (currentWeaponIndex + (int)Input.mouseScrollDelta.y) % weapons.Count;
        if(newWeaponIndex < 0) newWeaponIndex = weapons.Count - 1;

        if(newWeaponIndex != currentWeaponIndex)
        {
            weapons[currentWeaponIndex].gameObject.SetActive(false);
            weapons[currentWeaponIndex].OnTargetKill -= OnEnemyKill;

            weapons[newWeaponIndex].gameObject.SetActive(true);
            currentWeaponIndex = newWeaponIndex;

            weapons[currentWeaponIndex].damageMultiplier = damageMultiplier;
            weapons[currentWeaponIndex].OnTargetKill += OnEnemyKill;
        }

        if (Input.GetKey(KeyCode.Mouse0) && weapons[currentWeaponIndex].isSemiAuto || Input.GetKeyDown(KeyCode.Mouse0))
        {
            weapons[currentWeaponIndex].OnAttack(cam.position, cam.forward);
        }

        if (Input.GetKey(KeyCode.R))
        {
            weapons[currentWeaponIndex].Reload();
        }
    }

    void AddWeapon(WeaponTemplate newWeaponPrefab)
    {
        weapons.Add(Instantiate(newWeaponPrefab, weaponsContent));

        int newWeaponIndex = weapons.Count - 1;
        weapons[currentWeaponIndex].gameObject.SetActive(false);
        weapons[newWeaponIndex].gameObject.SetActive(true);
        currentWeaponIndex = newWeaponIndex;
    }

    void AddDamageMultiplier(float multToAdd)
    {
        damageMultiplier += multToAdd;
    }
    void OnEnemyKill()
    {
        print("kill enemy");
    }
}