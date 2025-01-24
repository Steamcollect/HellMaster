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
    int scoreMult = 1;
    int currentScoreCount;

    [Header("References")]
    [SerializeField] Transform content;
    [SerializeField] TMP_Text scoreTxt;
    [SerializeField] TMP_Text scoreGivenTxt;
    [SerializeField] TMP_Text scoreMultTxt;
    TMP_Text currentScoreGivenTxt;
    [SerializeField] TMP_Text bestScoreTxt;
    Color initMultColor;

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
        initMultColor = scoreMultTxt.color;
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
                scoreMult = 1;
                currentScoreCount = 0;
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
        currentScoreCount++;

        if (this.scoreGiven == 0)
        {
            initScore = score;
            currentScoreGivenTxt = Instantiate(scoreGivenTxt, content);
            currentScoreGivenTxt.transform.position = scoreGivenTxt.transform.position;
            currentScoreGivenTxt.transform.rotation = scoreGivenTxt.transform.rotation;
        }
        giveScoreTimer = 0;

        if(currentScoreCount > 10)
        {
            int newScoreMult = currentScoreCount / 10;
            if (newScoreMult > scoreMult)
            {

                scoreMultTxt.text = "x" + newScoreMult;
                scoreMult = newScoreMult;

                scoreMultTxt.DOKill();
                scoreMultTxt.transform.DOKill();

                scoreMultTxt.color = initMultColor;
                scoreMultTxt.transform.BumpBigVisual();

                scoreMultTxt.transform.SetAsLastSibling();
            }
        }

        this.scoreGiven += scoreGiven;

        currentScoreGivenTxt.transform.DOKill();
        currentScoreGivenTxt.transform.BumpVisual();
        currentScoreGivenTxt.text = this.scoreGiven.ToString("#,0");

        score += scoreGiven * scoreMult;
        if(rsoContentSaved.Value.bestScore < score)
        {
            bestScoreTxt.text = "Best: " + score.ToString("#,0");
            rsoContentSaved.Value.bestScore = score;
            rseSaveData.Call();
        }

        scoreTxt.transform.DOKill();
        scoreTxt.transform.BumpVisual();
    }

    void GiveScoreVisual(TMP_Text txt, int finalScore, int initScore)
    {
        int currentScore = initScore;

        scoreMultTxt.transform.DOScale(1.3f, .5f);
        scoreMultTxt.DOFade(0, .5f);

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