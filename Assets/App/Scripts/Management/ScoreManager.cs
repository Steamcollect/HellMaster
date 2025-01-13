using UnityEngine;
using DG.Tweening;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int score;

    [Header("References")]
    [SerializeField] TMP_Text scoreTxt;
    [SerializeField] TMP_Text bestScoreTxt;

    //[Space(10)]
    // RSO
    [SerializeField] RSO_ContentSaved rsoContentSaved;
    // RSF
    // RSP

    [Header("Input")]
    [SerializeField] RSE_AddScore rseAddScore;

    [Header("Output")]
    [SerializeField] RSE_SaveData rseSaveData;

    private void OnEnable()
    {
        rseAddScore.action += AddScore;
    }
    private void OnDisable()
    {
        rseAddScore.action -= AddScore;
    }

    private void Start()
    {
        Invoke("LateStart", .15f);
    }
    
    void LateStart()
    {
        bestScoreTxt.text = "Best: " + rsoContentSaved.Value.bestScore.ToString("#,0");
    }

    void AddScore(int scoreGiven)
    {
        score += scoreGiven;
        scoreTxt.text = score.ToString("#,0");
        if(rsoContentSaved.Value.bestScore < score)
        {
            rsoContentSaved.Value.bestScore = score;
            bestScoreTxt.text = "Best: " + score.ToString("#,0");
            rseSaveData.Call();
        }

        scoreTxt.transform.BumpVisual();
    }
}