using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcess : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] Volume globalVolum;

    Vignette vignette;
    Sequence vignetteSequence;
    float vignetteIntensityValue;

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    //[Header("Input")]

    [Header("Output")]
    [SerializeField] RSE_OnShoot rseOnShoot;

    private void OnEnable()
    {
        rseOnShoot.action += BumpVignetteValue;
    }
    private void OnDisable()
    {
        rseOnShoot.action -= BumpVignetteValue;
    }

    private void Awake()
    {
        if (globalVolum.profile.TryGet(out Vignette _vignette)) vignette = _vignette;
    }

    private void Start()
    {
        vignetteIntensityValue = vignette.intensity.value;
    }

    void BumpVignetteValue()
    {
        DOTween.Kill(vignetteSequence);

        vignetteSequence = DOTween.Sequence(
            DOTween.To(() => vignetteIntensityValue, x => vignetteIntensityValue = x, 
                0.2f/*End Value*/ , 
                0.1f/*Time*/)
                    .OnUpdate(() =>
                    {
                        vignette.intensity.value = vignetteIntensityValue;
                    })
                    .OnComplete(() =>
                    {

                        DOTween.To(() => vignetteIntensityValue, x => vignetteIntensityValue = x,
                            0/*End Value*/ ,
                            0.1f/*Time*/)
                                .OnUpdate(() =>
                                {
                                    vignette.intensity.value = vignetteIntensityValue;
                                });
                    })
            );
    }
}