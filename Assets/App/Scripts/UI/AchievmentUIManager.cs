using UnityEngine;
using System.Collections.Generic;

public class AchievmentUIManager : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] Achievment[] achievments;

    [Space(10)]
    [SerializeField] Transform achievmentMenuUIContent;
    [SerializeField] Transform achievmentGameUIContent;
    [SerializeField] AchievmentUI achievmentUIPrefab;

    [Space(10)]
    // RSO
    [SerializeField] RSO_ContentSaved rsoContentSave;
    // RSF
    // RSP

    [Header("Input")]
    [SerializeField] RSE_ShowAchievmentOnScreen rseShowAchievmentOnScreen;

    //[Header("Output")]

    [Space(10)]
    [SerializeField] List<AchievmentUISetup> achievmentUISetup = new();

    public struct AchievmentUISetup
    {
        Achievment achievment;
        AchievmentUI ui;
        public AchievmentUISetup(Achievment achievment, AchievmentUI ui)
        {
            this.achievment = achievment;
            this.ui = ui;
        }
    }

    private void OnEnable()
    {
        rseShowAchievmentOnScreen.action += ShowAchievment;
    }
    private void OnDisable()
    {
        rseShowAchievmentOnScreen.action -= ShowAchievment;
    }
    
    private void Start()
    {
        Invoke("LateStart", .01f);
    }

    void LateStart()
    {
        if(rsoContentSave.Value.achievments.Length < achievments.Length)
        {
            rsoContentSave.Value.achievments = achievments;
        }
        else
        {
            achievments = rsoContentSave.Value.achievments;
        }

        foreach (var achievment in achievments)
        {
            AchievmentUI ui = Instantiate(achievmentUIPrefab, achievmentMenuUIContent);
            ui.Setup(achievment);
            achievmentUISetup.Add(new AchievmentUISetup(achievment, ui));
        }
    }

    void ShowAchievment(Achievment achievment)
    {
        AchievmentUI ui = Instantiate(achievmentUIPrefab, achievmentGameUIContent);
        ui.Setup(achievment);
        Destroy(ui.gameObject, 5f);
    }
}