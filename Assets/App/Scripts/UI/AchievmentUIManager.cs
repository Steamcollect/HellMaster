using UnityEngine;
using System.Collections.Generic;

public class AchievmentUIManager : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] Achievment[] achievments;

    [Space(10)]
    [SerializeField] Transform achievmentUIContent;
    [SerializeField] AchievmentUI achievmentUIPrefab;

    [Space(10)]
    // RSO
    [SerializeField] RSO_ContentSaved rsoContentSave;
    // RSF
    // RSP

    //[Header("Input")]
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

        foreach (var item in achievments)
        {
            AchievmentUI ui = Instantiate(achievmentUIPrefab, achievmentUIContent);
            ui.Setup(item);
            achievmentUISetup.Add(new AchievmentUISetup(item, ui));
        }
    }
}