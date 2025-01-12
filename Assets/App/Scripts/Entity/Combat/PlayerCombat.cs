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
    [SerializeField] RSE_AddWeapon rseAddWeapon;
    [SerializeField] RSE_OnPauseStateChanged rseOnPauseStateChanged;

    //[Header("Output")]

    private void OnEnable()
    {
        rseAddDamageMult.action += AddDamageMultiplier;
        rseOnGameStart.action += OnGameStart;
        rseOnPlayerDeath.action += OnPlayerDeath;
        rseSaveGameData.action += SaveGameData;
        rseAddWeapon.action += AddWeapon;
        rseOnPauseStateChanged.action += OnPauseStateChanged;
    }
    private void OnDisable()
    {
        rseAddDamageMult.action -= AddDamageMultiplier;
        rseOnGameStart.action -= OnGameStart;
        rseOnPlayerDeath.action -= OnPlayerDeath;
        rseSaveGameData.action -= SaveGameData;
        rseAddWeapon.action -= AddWeapon;
        rseOnPauseStateChanged.action -= OnPauseStateChanged;
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
                weapons[currentWeaponIndex].Hide();
                weapons[currentWeaponIndex].OnTargetKill -= OnEnemyKill;

                weapons[newWeaponIndex].Show();
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
        weapons[currentWeaponIndex].Hide();
        weapons[newWeaponIndex].Show();
        currentWeaponIndex = newWeaponIndex;
    }

    void AddDamageMultiplier(float multToAdd)
    {
        damageMultiplier += multToAdd;
    }
    void OnEnemyKill()
    {
        totalEnemysKilled++;
        foreach (var item in achievmentsKillEnemys)
        {
            item.AddEnemysKilled(totalEnemysKilled);
        }
    }

    void OnPauseStateChanged(bool isPaused)
    {
        canAttack = !isPaused;
    }
}