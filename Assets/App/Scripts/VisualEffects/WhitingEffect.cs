using DG.Tweening;
using System.Collections;
using UnityEngine;
public class WhitingEffect : MonoBehaviour
{
    float whitingValue;
    [SerializeField] float whithingTime;

    Sequence whitingSequence;

    [Header("References")]
    [SerializeField] private SkinnedMeshRenderer meshRenderer;

    public void Tinting()
    {
        DOTween.Kill(whitingSequence);

        whitingSequence = DOTween.Sequence(
            DOTween.To(() => whitingValue, x => whitingValue = x, 1, whithingTime)
            .OnUpdate(() =>
            {
                foreach (Material mat in meshRenderer.materials)
                {
                    Debug.Log(mat.ToString());
                    mat.SetFloat("_Whitness", whitingValue);
                }
            }).OnComplete(() =>
            {
                DOTween.To(() => whitingValue, x => whitingValue = x, 0, whithingTime)
                .OnUpdate(() =>
                {
                    foreach (Material mat in meshRenderer.materials)
                    {
                        mat.SetFloat("_Whitness", whitingValue);
                    }
                });
            })
            );        
    }

    public void OnDisable()
    {
        DOTween.Kill(whitingSequence);
    }
}