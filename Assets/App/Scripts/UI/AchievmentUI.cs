using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class AchievmentUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Color lockColor, unlockColor;

    [Header("References")]
    [SerializeField] Image iconImage;
    [SerializeField] Image backgroundImage;
    [SerializeField] TMP_Text titleTxt;
    [SerializeField] TMP_Text descriptionTxt;

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    //[Header("Input")]
    //[Header("Output")]

    public void Setup(Achievment achievment)
    {
        iconImage.sprite = achievment.achievmentIcon;
        backgroundImage.color = achievment.isAchieve ? unlockColor : lockColor;

        titleTxt.text = achievment.achivmentName;
        descriptionTxt.text = achievment.achivmentDescription;
    }
}