using System;
using System.Linq;
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
    float attackRateMultiplier = 1;

    bool canAttack = false;

    [Space(10)]
    [SerializeField] AudioClip[] swapWeaponSounds;
    [SerializeField] AudioClip[] enemyKillSound;
    [SerializeField] RSE_PlayClipAt rsePlayClipAt;

    [Header("Achievments")]
    List<SSO_Achievment_KillEnemys> achievmentsKillEnemys = new();
    [SerializeField] SSO_Achivment_CompleteOnce reloadAchivment;

    // RSO
    [SerializeField] RSO_ContentSaved rsoContentSave;
    [SerializeField] RSO_Achievements rsoAchievements;
    // RSF
    // RSP

    [Header("Input")]
    [SerializeField] RSE_AddDamageMultiplier rseAddDamageMult;
    [SerializeField] RSE_AddAttackRateMultiplier rseAddAttackRateMult;
    [SerializeField] RSE_OnGameStart rseOnGameStart;
    [SerializeField] RSE_OnPlayerDeath rseOnPlayerDeath;
    [SerializeField] RSE_SaveAllGameData rseSaveGameData;
    [SerializeField] RSE_AddWeapon rseAddWeapon;
    [SerializeField] RSE_OnPauseStateChanged rseOnPauseStateChanged;

    //[Header("Output")]

    private void OnEnable()
    {
        rseAddDamageMult.action += AddDamageMultiplier;
        rseAddAttackRateMult.action += AddAttackRateMultiplier;
        rseOnGameStart.action += OnGameStart;
        rseOnPlayerDeath.action += OnPlayerDeath;
        rseSaveGameData.action += SaveGameData;
        rseAddWeapon.action += AddWeapon;
        rseOnPauseStateChanged.action += OnPauseStateChanged;
    }
    private void OnDisable()
    {
        rseAddDamageMult.action -= AddDamageMultiplier;
        rseAddAttackRateMult.action -= AddAttackRateMultiplier;
        rseOnGameStart.action -= OnGameStart;
        rseOnPlayerDeath.action -= OnPlayerDeath;
        rseSaveGameData.action -= SaveGameData;
        rseAddWeapon.action -= AddWeapon;
        rseOnPauseStateChanged.action -= OnPauseStateChanged;
    }

    void OnGameStart()
    {
        foreach (var achievement in rsoAchievements.Value)
        {
            if(achievement is SSO_Achievment_KillEnemys achievementKillEnemy)
            {
                achievmentsKillEnemys.Add(achievementKillEnemy);
            }
        }

        totalEnemysKilled = rsoContentSave.Value.totalEnemysKilled;
        weapons[currentWeaponIndex].damageMultiplier = damageMultiplier;
        weapons[currentWeaponIndex].attackRateMultiplier = attackRateMultiplier;        
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
                rsePlayClipAt.Call(swapWeaponSounds.GetRandom(), transform.position, 1);
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
                reloadAchivment.Achieve();
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
    void AddAttackRateMultiplier(float multToAdd)
    {
        attackRateMultiplier += multToAdd;
    }
    void OnEnemyKill()
    {
        rsePlayClipAt.Call(enemyKillSound.GetRandom(), transform.position, 1);
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