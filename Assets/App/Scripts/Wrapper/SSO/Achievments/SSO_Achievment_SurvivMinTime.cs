using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SSO_Achievment_SurvivMinTime", menuName = "ScriptableObject/SSO_Achievment_SurvivMinTime")]
public class SSO_Achievment_SurvivMinTime : Achievment
{
    [SerializeField] float timeRequire;
    [SerializeField] int maxHealthGiven;

    [SerializeField] RSE_AddPlayerMaxHealth rseAddMaxHealth;

    public Coroutine delayTimer;
    public IEnumerator Delay()
    {
        if (!isAchieve)
        {
            yield return new WaitForSeconds(timeRequire);

            rseAddMaxHealth.Call(maxHealthGiven);
            isAchieve = true;
            rseSaveAllGameData.Call();
        }
    }

    public override void GiveBonnus()
    {
        if (isAchieve)
        {
            rseAddMaxHealth.Call(maxHealthGiven);
        }
    }
}