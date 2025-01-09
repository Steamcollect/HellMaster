using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SSO_Achievment_KillEnemys", menuName = "ScriptableObject/SSO_Achievment_KillEnemys")]
public class SSO_Achievment_KillEnemys : Achievment
{
    [SerializeField] int killRequired;

    [SerializeField] float damageMultGiven;

    [SerializeField] RSE_AddDamageMultiplier rseAddDamageMult;

    public void AddEnemysKilled(int kill)
    {
        if(kill >= killRequired && !isAchieve)
        {
            Debug.Log(kill);
            isAchieve = true;
            rseAddDamageMult.Call(damageMultGiven);
            rseSaveAllGameData.Call();
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