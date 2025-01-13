using UnityEngine;
using System.Collections.Generic;

public class AchievmentUIManager : MonoBehaviour
{
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
    [SerializeField] RSE_OnAchievmentComplete rseShowAchievmentOnScreen;
    [SerializeField] RSE_OnPanelOpen rsePanelOpen;

    [Space(10)]
    [SerializeField] List<AchievmentUISetup> achievmentUISetup = new();

    public struct AchievmentUISetup
    {
        public Achievment achievment;
        public AchievmentUI ui;
        public AchievmentUISetup(Achievment achievment, AchievmentUI ui)
        {
            this.achievment = achievment;
            this.ui = ui;
        }
    }

    private void OnEnable()
    {
        rseShowAchievmentOnScreen.action += ShowAchievment;
        rsePanelOpen.action += OnPanelOpen;
    }
    private void OnDisable()
    {
        rseShowAchievmentOnScreen.action -= ShowAchievment;
        rsePanelOpen.action -= OnPanelOpen;
    }

    private void Start()
    {
        Invoke("LateStart", .1f);
    }

    void LateStart()
    {
        for (int i = 0; i < achievments.Length; i++)
        {
            achievments[i].isAchieve = rsoContentSave.Value.achievmentsStatus[i];
            AchievmentUI ui = Instantiate(achievmentUIPrefab, achievmentMenuUIContent);
            ui.Setup(achievments[i]);
            achievmentUISetup.Add(new AchievmentUISetup(achievments[i], ui));
        }
    }

    void OnPanelOpen()
    {
        foreach (var achivment in achievmentUISetup)
        {
            achivment.ui.Setup(achivment.achievment);
        }
    }

    void ShowAchievment(Achievment achievment)
    {
        AchievmentUI ui = Instantiate(achievmentUIPrefab, achievmentGameUIContent);
        ui.Setup(achievment);
        Destroy(ui.gameObject, 5f);
    }
}