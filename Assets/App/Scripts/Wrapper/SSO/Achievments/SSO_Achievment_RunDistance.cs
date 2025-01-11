using UnityEngine;

[CreateAssetMenu(fileName = "SSO_Achievment_RunDistance", menuName = "ScriptableObject/SSO_Achievment_RunDistance")]
public class SSO_Achievment_RunDistance : Achievment
{
    [SerializeField] float distanceRequire;
    [SerializeField] float moveSpeedMult;

    [SerializeField] RSE_AddMoveSpeedMultiplier rseAddMoveSpeedMult;
    [SerializeField] RSO_AchievmentCompleteCount rsoAchievmentCompleteCount;

    public void CheckDistance(float distance)
    {
        if (!isAchieve && distance >= distanceRequire)
        {
            rseAddMoveSpeedMult.Call(moveSpeedMult);
            isAchieve = true;
            rseSaveAllGameData.Call();
            rsoAchievmentCompleteCount.Value++;
        }
    }

    public override void GiveBonnus()
    {
        if (isAchieve)
        {
            rseAddMoveSpeedMult.Call(moveSpeedMult);
            rsoAchievmentCompleteCount.Value++;
        }
    }
}