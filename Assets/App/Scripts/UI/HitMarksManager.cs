using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class HitMarksManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float hitMarksTime;

    [Header("References")]
    [SerializeField] Animator hitMarksPrefab;
    Camera cam;

    IObjectPool<Animator> pool;

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    [Header("Input")]
    [SerializeField] RSE_OnEnemyHit rseOnEnemyHit;

    //[Header("Output")]

    private void OnEnable()
    {
        rseOnEnemyHit.action += ShowHitMark;
    }
    private void OnDisable()
    {
        rseOnEnemyHit.action -= ShowHitMark;
    }

    private void Awake()
    {
        cam = Camera.main;
        pool = new ObjectPool<Animator>(CreateHitMarks);
    }

    void ShowHitMark(Vector3 position)
    {
        Animator hitMark = pool.Get();
        hitMark.gameObject.SetActive(true);
        hitMark.transform.position = cam.WorldToScreenPoint(position);

        StartCoroutine(Delay(hitMark));
    }

    IEnumerator Delay(Animator hitMark)
    {
        hitMark.SetTrigger("Show");
        yield return new WaitForSeconds(hitMarksTime);
        hitMark.gameObject.SetActive(false);
        pool.Release(hitMark);
    }

    Animator CreateHitMarks()
    {
        return Instantiate(hitMarksPrefab, transform);
    }
}