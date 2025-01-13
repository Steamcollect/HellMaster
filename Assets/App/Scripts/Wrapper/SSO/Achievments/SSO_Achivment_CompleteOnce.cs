using UnityEngine;

[CreateAssetMenu(fileName = "SSO_Achivment_CompleteOnce", menuName = "ScriptableObject/SSO_Achivment_CompleteOnce")]
public class SSO_Achivment_CompleteOnce : Achievment
{
    public void Achieve()
    {
        if (!isAchieve)
        {
            OnComplete();
        }
    }

    public override void GiveBonnus()
    {
        rsoAchievmentCompleteCount.Value++;
    }
}