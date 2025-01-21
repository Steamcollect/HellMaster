using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class HitMark : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]float targetSize;
    [SerializeField] float bigScale;
    [SerializeField] float bigScaleTime;

    [Space(5)]
    [SerializeField] float disparitionTime;

    [Header("References")]
    Camera cam;
    [HideInInspector] public IObjectPool<HitMark> pool;

    //[Space(10)]
    // RSO
    [SerializeField] RSO_PlayerPosition rsoPlayerPos;
    // RSF
    // RSP

    //[Header("Input")]
    //[Header("Output")]

    public void Start()
    {
        cam = Camera.main;

        float distance = Vector3.Distance(transform.position, rsoPlayerPos.Value);

        // Calculer la taille de l'objet en fonction de la distance
        float size = 2 * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        transform.localScale = Vector3.one * (targetSize / size);
        transform.LookAt(cam.transform.position);

        transform.DOScale(targetSize * bigScale, bigScaleTime).OnComplete(() =>
        {
            transform.DOScale(0, disparitionTime).OnComplete(()=>
            {
                pool.Release(this);
            });
        });
    }
}