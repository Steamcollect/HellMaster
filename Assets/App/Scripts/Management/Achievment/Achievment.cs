using UnityEngine;

public class Achievment : ScriptableObject
{
    public string achivmentName;
    [TextArea] public string achivmentDescription;
    public Sprite achievmentIcon;

    [HideInInspector] public bool isAchieve;
}