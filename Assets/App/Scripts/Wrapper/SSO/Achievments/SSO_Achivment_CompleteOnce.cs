using UnityEngine;

[CreateAssetMenu(fileName = "SSO_Achivment_CompleteOnce", menuName = "ScriptableObject/SSO_Achivment_CompleteOnce")]
public class SSO_Achivment_CompleteOnce : Achievment
{
    [SerializeField] RSE_ActiveDoubleJump rseDoubleJump;
    [SerializeField] bool canActiveDoubleJumps;

    public void Achieve()
    {
        if (!isAchieve)
        {
            if (canActiveDoubleJumps) rseDoubleJump.Call();
            OnComplete();
        }
    }

    public override void GiveBonnus()
    {
        rsoAchievmentCompleteCount.Value++;
    }
}