using UnityEngine;

[CreateAssetMenu(fileName = "SSO_Achievment_JumpCount", menuName = "ScriptableObject/SSO_Achievment_JumpCount")]
public class SSO_Achievment_JumpCount : Achievment
{
    [SerializeField] int jumpCountRequire;

    public void CheckJumpCount(int jumpCount)
    {
        if(!isAchieve && jumpCount >= jumpCountRequire)
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