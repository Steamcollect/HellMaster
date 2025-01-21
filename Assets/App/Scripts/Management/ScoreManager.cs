using UnityEngine;
using DG.Tweening;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int score;
    [SerializeField] int scoreGiven;
    int initScore;
    [SerializeField] float giveScoreTime;
    float giveScoreTimer;

    [Header("References")]
    [SerializeField] Transform content;
    [SerializeField] TMP_Text scoreTxt;
    [SerializeField] TMP_Text scoreGivenTxt;
    TMP_Text currentScoreGivenTxt;
    [SerializeField] TMP_Text bestScoreTxt;

    //[Space(10)]
    // RSO
    [SerializeField] RSO_ContentSaved rsoContentSaved;
    // RSF
    // RSP

    [Header("Input")]
    [SerializeField] RSE_AddScore rseAddScore;
    [SerializeField] RSE_OnEnemyHit rseOnEnemyHit;

    [Header("Output")]
    [SerializeField] RSE_SaveData rseSaveData;

    private void OnEnable()
    {
        rseAddScore.action += AddScore;
        rseOnEnemyHit.action += AddOne;
    }
    private void OnDisable()
    {
        rseAddScore.action -= AddScore;
        rseOnEnemyHit.action -= AddOne;
    }

    private void Start()
    {
        Invoke("LateStart", .15f);
    }

    private void Update()
    {
        if (scoreGiven > 0)
        {
            giveScoreTimer += Time.deltaTime;
            if(giveScoreTimer > giveScoreTime)
            {
                GiveScoreVisual(currentScoreGivenTxt, score, initScore);
                scoreGiven = 0;
                giveScoreTimer = 0;
            }
        }
    }

    void LateStart()
    {
        bestScoreTxt.text = "Best: " + rsoContentSaved.Value.bestScore.ToString("#,0");
    }

    void AddOne(Vector3 pos) => AddScore(1);
    void AddScore(int scoreGiven)
    {
        if (this.scoreGiven == 0)
        {
            initScore = score;
            currentScoreGivenTxt = Instantiate(scoreGivenTxt, content);
            currentScoreGivenTxt.transform.position = scoreGivenTxt.transform.position;
            currentScoreGivenTxt.transform.rotation = scoreGivenTxt.transform.rotation;
        }
        giveScoreTimer = 0;
        this.scoreGiven += scoreGiven;
        currentScoreGivenTxt.transform.BumpVisual();
        currentScoreGivenTxt.text = this.scoreGiven.ToString("#,0");

        score += scoreGiven;
        if(rsoContentSaved.Value.bestScore < score)
        {
            bestScoreTxt.text = "Best: " + score.ToString("#,0");
            rsoContentSaved.Value.bestScore = score;
            rseSaveData.Call();
        }

        scoreTxt.transform.BumpVisual();
    }

    void GiveScoreVisual(TMP_Text txt, int finalScore, int initScore)
    {
        int currentScore = initScore;
        txt.transform.DOMove(scoreTxt.transform.position, .3f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            Destroy(txt.gameObject);

            scoreTxt.transform.BumpBigVisual();
            DOTween.To(() => currentScore, x => currentScore = x, finalScore, .5f)
                .OnUpdate(() => {
                    scoreTxt.text = currentScore.ToString("#,0");
                });
            
        });
    }
}