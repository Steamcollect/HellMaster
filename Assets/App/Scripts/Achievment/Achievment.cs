using UnityEngine;

public abstract class Achievment : ScriptableObject
{
    public string achivmentName;
    [TextArea] public string achivmentDescription;
    public Sprite achievmentIcon;

    public bool isAchieve;

    public RSO_AchievmentCompleteCount rsoAchievmentCompleteCount;

    public RSE_SaveAllGameData rseSaveAllGameData;
    public RSE_OnAchievmentComplete rseShowAchievmentOnScreen;

    public abstract void GiveBonnus();

    public void OnComplete()
    {
        isAchieve = true;
        rseSaveAllGameData.Call();
        rsoAchievmentCompleteCount.Value++;
        rseShowAchievmentOnScreen.Call(this);
    }
}