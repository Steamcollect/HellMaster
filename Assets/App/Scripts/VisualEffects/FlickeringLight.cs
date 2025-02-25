using UnityEngine;
public class FlickeringLight : MonoBehaviour
{
    [SerializeField] Light _light;
    [SerializeField] float maxInterval = 1f;
    [SerializeField] float minIntensity;
    [SerializeField] float maxIntensity;

    float targetIntensity;
    float lastIntensity;
    float interval;
    float timer;

    public float maxDisplacement = 0.25f;
    Vector3 targetPosition;
    Vector3 lastPosition;
    Vector3 origin;

    private void Start()
    {
        origin = _light.transform.position;
        lastPosition = origin;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > interval)
        {
            lastIntensity = _light.intensity;
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            timer = 0;
            interval = Random.Range(0, maxInterval);

            targetPosition = origin + Random.insideUnitSphere * maxDisplacement;
            lastPosition = _light.transform.position;
        }

        _light.intensity = Mathf.Lerp(lastIntensity, targetIntensity, timer / interval);
        _light.transform.position = Vector3.Lerp(lastPosition, targetPosition, timer / interval);
    }
}