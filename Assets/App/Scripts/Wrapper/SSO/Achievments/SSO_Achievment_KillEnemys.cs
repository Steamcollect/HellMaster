using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SSO_Achievment_KillEnemys", menuName = "ScriptableObject/SSO_Achievment_KillEnemys")]
public class SSO_Achievment_KillEnemys : Achievment
{
    int enemysKilled = 0;
    [SerializeField] int killRequired;

    [SerializeField] float damageMultGiven;

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

    public override void GiveBonnus()
    {
        if (isAchieve)
        {
            rseAddDamageMult.Call(damageMultGiven);
        }
    }
}