using UnityEngine;
using System.Collections.Generic;

public class AchievmentUIManager : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public Achievment[] achievments;

    [Space(10)]
    [SerializeField] Transform achievmentMenuUIContent;
    [SerializeField] Transform achievmentGameUIContent;
    [SerializeField] AchievmentUI achievmentUIPrefab;
    [SerializeField] AudioClip[] showAchievementSounds;

    [Space(10)]
    // RSO
    [SerializeField] RSO_ContentSaved rsoContentSave;
    [SerializeField] RSO_Achievements rsoAchievements;
    // RSF
    // RSP

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

    [Header("Input")]
    [SerializeField] RSE_OnAchievmentComplete rseShowAchievmentOnScreen;
    [SerializeField] RSE_OnPanelOpen rsePanelOpen;
    [SerializeField] RSE_OnGameStart rseOnGameStart;

    [Header("Output")]
    [SerializeField] RSE_PlayClipAt rsePlayClipAt;

    private void OnEnable()
    {
        rseShowAchievmentOnScreen.action += ShowAchievment;
        rsePanelOpen.action += OnPanelOpen;
        rseOnGameStart.action += CheckAchievmentCompleteWithDelay;
    }
    private void OnDisable()
    {
        rseShowAchievmentOnScreen.action -= ShowAchievment;
        rsePanelOpen.action -= OnPanelOpen;
        rseOnGameStart.action -= CheckAchievmentCompleteWithDelay;
    }

    private void Start()
    {
        Invoke("LateStart", .1f);
    }

    void LateStart()
    {
        achievments = rsoAchievements.Value;
        for (int i = 0; i < achievments.Length; i++)
        {
            achievments[i].isAchieve = rsoContentSave.Value.achievmentsStatus[i];
            AchievmentUI ui = Instantiate(achievmentUIPrefab, achievmentMenuUIContent);
            ui.Setup(achievments[i]);
            achievmentUISetup.Add(new AchievmentUISetup(achievments[i], ui));
        }
    }

    void CheckAchievmentCompleteWithDelay()
    {
        Invoke("CheckAchievmentComplete", .1f);
    }
    void CheckAchievmentComplete()
    {
        foreach (var item in achievments)
        {
            item.GiveBonnus();
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
        rsePlayClipAt.Call(showAchievementSounds.GetRandom(), transform.position, 0);
        AchievmentUI ui = Instantiate(achievmentUIPrefab, achievmentGameUIContent);
        ui.Setup(achievment);
        Destroy(ui.gameObject, 5f);
    }
}