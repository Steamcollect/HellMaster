using UnityEngine;

[CreateAssetMenu(fileName = "SSO_Achievment_AireTimeJump", menuName = "ScriptableObject/SSO_Achievment_AireTimeJump")]
public class SSO_Achievment_AireTimeJump : Achievment
{
    [SerializeField] float aireTimeRequire;
    [SerializeField] RSE_AddAttackRateMultiplier rseAddAttackRateMult;
    [SerializeField] float valueGiven;

    public void CheckAirTime(float aireTime)
    {
        if (!isAchieve && aireTimeRequire <= aireTime)
        {
            rseAddAttackRateMult.Call(valueGiven);
            OnComplete();
        }
    }

    public override void GiveBonnus()
    {
        if (isAchieve)
        {
            rseAddAttackRateMult.Call(valueGiven);
            rsoAchievmentCompleteCount.Value++;
        }
    }
}