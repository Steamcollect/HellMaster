using UnityEngine;
using UnityEngine.Pool;
public class HitMarksManager : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] HitMark HitMarkPrefab;

    IObjectPool<HitMark> pool;

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

    private void Awake()
    {
        pool = new ObjectPool<HitMark>(Create);
    }

    void Show(Vector3 position)
    {
        HitMark mark = pool.Get();
        mark.pool = pool;
        mark.transform.position = position;
    }

    HitMark Create()
    {
        return Instantiate(HitMarkPrefab).GetComponent<HitMark>();
    }
}