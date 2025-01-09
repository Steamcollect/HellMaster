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

    int totalEnemysKilled;
    float damageMultiplier = 1;

    bool canAttack = false;

    [Header("Achievments")]
    [SerializeField] SSO_Achievment_KillEnemys[] achievmentsKillEnemys;

    // RSO
    [SerializeField] RSO_ContentSaved rsoContentSave;
    // RSF
    // RSP

    [Header("Input")]
    [SerializeField] RSE_AddDamageMultiplier rseAddDamageMult;
    [SerializeField] RSE_OnGameStart rseOnGameStart;
    [SerializeField] RSE_OnPlayerDeath rseOnPlayerDeath;
    [SerializeField] RSE_SaveAllGameData rseSaveGameData;

    //[Header("Output")]

    private void OnEnable()
    {
        rseAddDamageMult.action += AddDamageMultiplier;
        rseOnGameStart.action += OnGameStart;
        rseOnPlayerDeath.action += OnPlayerDeath;
        rseSaveGameData.action += SaveGameData;
    }
    private void OnDisable()
    {
        rseAddDamageMult.action -= AddDamageMultiplier;
        rseOnGameStart.action -= OnGameStart;
        rseOnPlayerDeath.action -= OnPlayerDeath;
        rseSaveGameData.action -= SaveGameData;
    }

    void OnGameStart()
    {
        totalEnemysKilled = rsoContentSave.Value.totalEnemysKilled;
        weapons[currentWeaponIndex].damageMultiplier = damageMultiplier;
        weapons[currentWeaponIndex].OnTargetKill += OnEnemyKill;
        canAttack = true;
    }
    void OnPlayerDeath()
    {
        rsoContentSave.Value.totalEnemysKilled = totalEnemysKilled;
        canAttack = false;
    }
    void SaveGameData()
    {
        rsoContentSave.Value.totalEnemysKilled = totalEnemysKilled;
    }
    private void Update()
    {
        if (canAttack)
        {
            int newWeaponIndex = (currentWeaponIndex + (int)Input.mouseScrollDelta.y) % weapons.Count;
            if (newWeaponIndex < 0) newWeaponIndex = weapons.Count - 1;

            if (newWeaponIndex != currentWeaponIndex)
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
        totalEnemysKilled++;
        print(totalEnemysKilled);
        foreach (var item in achievmentsKillEnemys)
        {
            item.AddEnemysKilled(totalEnemysKilled);
        }
    }
}