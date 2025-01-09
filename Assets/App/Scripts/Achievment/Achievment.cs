using UnityEngine;

public abstract class Achievment : ScriptableObject
{
    public string achivmentName;
    [TextArea] public string achivmentDescription;
    public Sprite achievmentIcon;

    public bool isAchieve;

    public abstract void GiveBonnus();
}