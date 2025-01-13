using UnityEngine;

[CreateAssetMenu(fileName = "SSO_Achivment_HealCount", menuName = "ScriptableObject/SSO_Achivment_HealCount")]
public class SSO_Achivment_HealCount : Achievment
{
    [SerializeField] int healCountRequire;

    public void CheckHealCount(int healCount)
    {
        if(!isAchieve && healCount >= healCountRequire)
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