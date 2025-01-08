using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SSO_Achievment_Kill1Enemy", menuName = "ScriptableObject/SSO_Achievment_Kill1Enemy")]
public class SSO_Achievment_Kill1Enemy : Achievment
{
    int enemysKilled = 0;
    public int killRequired;

    public float damageMultGiven;

    [SerializeField] RSE_AddDamageMultiplier rseAddDamageMult;

    public void AddEnemysKilled(int kill)
    {
        enemysKilled += kill;

        if(enemysKilled >= killRequired && !isAchieve)
        {
            isAchieve = true;
            rseAddDamageMult.Call(damageMultGiven);
        }
    }
}