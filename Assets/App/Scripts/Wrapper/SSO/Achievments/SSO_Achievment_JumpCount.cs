using UnityEngine;

[CreateAssetMenu(fileName = "SSO_Achievment_JumpCount", menuName = "ScriptableObject/SSO_Achievment_JumpCount")]
public class SSO_Achievment_JumpCount : Achievment
{
    [SerializeField] int jumpCountRequire;
    [SerializeField] RSE_AddJumpForceMultiplier rseAddJumpForceMult;
    [SerializeField] float valueGiven;
    public void CheckJumpCount(int jumpCount)
    {
        if(!isAchieve && jumpCount >= jumpCountRequire)
        {
            rseAddJumpForceMult.Call(valueGiven);
            OnComplete();
        }
    }

    public override void GiveBonnus()
    {
        if (isAchieve)
        {
            rseAddJumpForceMult.Call(valueGiven);
            rsoAchievmentCompleteCount.Value++;
        }
    }
}