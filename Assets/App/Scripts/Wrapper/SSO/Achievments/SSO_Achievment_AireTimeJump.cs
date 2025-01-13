using UnityEngine;

[CreateAssetMenu(fileName = "SSO_Achievment_AireTimeJump", menuName = "ScriptableObject/SSO_Achievment_AireTimeJump")]
public class SSO_Achievment_AireTimeJump : Achievment
{
    [SerializeField] float aireTimeRequire;

    public void CheckAirTime(float aireTime)
    {
        if (!isAchieve && aireTimeRequire <= aireTime)
        {
            OnComplete();
        }
    }

    public override void GiveBonnus()
    {
        if (isAchieve)
        {
            rsoAchievmentCompleteCount.Value++;
        }
    }
}