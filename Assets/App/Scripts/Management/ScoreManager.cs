using UnityEngine;
using DG.Tweening;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    //[Header("Settings")]
    [SerializeField] int score;

    [Header("References")]
    [SerializeField] TMP_Text scoreTxt;

    //[Space(10)]
    // RSO
    [SerializeField] RSO_ContentSaved rsoContentSaved;
    // RSF
    // RSP

    //[Header("Input")]
    [SerializeField] RSE_AddScore rseAddScore;

    //[Header("Output")]

    private void OnEnable()
    {
        rseAddScore.action += AddScore;
    }
    private void OnDisable()
    {
        rseAddScore.action -= AddScore;
    }

    void AddScore(int scoreGiven)
    {
        score += scoreGiven;
        scoreTxt.text = score.ToString("#,0");
        if(rsoContentSaved.Value.bestScore > score)
        {
            rsoContentSaved.Value.bestScore = score;
        }

        scoreTxt.transform.BumpVisual();
    }
}