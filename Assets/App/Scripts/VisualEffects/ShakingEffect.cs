using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakingEffect : MonoBehaviour
{
    public bool start = false;
    bool isShaking = false;
    public AnimationCurve curve;
    public float duration = 1f;

    private void Update()
    {
        if(start && !isShaking)
        {
            start = false;
            isShaking = true;
            StartCoroutine(Shaking());

        }
    }

    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPosition;
        isShaking=false;
    }
}
