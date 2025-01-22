using UnityEngine;

public class HitMarksManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float minScale;
    [SerializeField] float maxScale;
    [SerializeField] float minRot;
    [SerializeField] float maxRot;

    [Header("References")]
    [SerializeField] Animator HitMarkAnimator;
    [SerializeField] GameObject HitMarkPrefab;

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    [Header("Input")]
    [SerializeField] RSE_OnEnemyHit rseOnEnemyHit;

    //[Header("Output")]

    private void OnEnable()
    {
        rseOnEnemyHit.action += Show;
    }
    private void OnDisable()
    {
        rseOnEnemyHit.action -= Show;
    }

    void Show(Vector3 position)
    {
        HitMarkPrefab.transform.localScale = Vector3.one;
        HitMarkPrefab.transform.localRotation = Quaternion.identity;
        HitMarkAnimator.SetTrigger("Show");
        float tempScale = Random.Range(minScale, maxScale);
        HitMarkPrefab.transform.localScale = new Vector3(tempScale, tempScale, tempScale);
        float tempRot = Random.Range(minRot, maxRot);
        HitMarkPrefab.transform.localRotation = Quaternion.Euler(0, 0, tempRot);

    }
}