using UnityEngine;

public class HitMarksManager : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] Animator HitMark;

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
        HitMark.SetTrigger("Show");
    }
}