using System;
using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine;
public class Weapon_Missile : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] private float speed = 10f;
    [SerializeField] private ParticleSystem smokeTrail;
    [SerializeField] bool missileFired = false;
    [SerializeField] Weapon_Explosion explosion;
    [SerializeField] RSE_CameraShake cameraShake;
    [SerializeField] float shakeRange;
    [SerializeField] float shakeTime;
    public float damage;
    public float damageMultiplier;
    public Action OnTargetKill;
    public Vector3 forward;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        Physics.IgnoreLayerCollision(6, 7, true);
        Physics.IgnoreLayerCollision(7, 7, true);
    }

    private void Update()
    {
        this.gameObject.transform.rotation = Quaternion.LookRotation(forward);
        if (!missileFired)
        {
            OnMissileFired();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("DontTouch"))
        {
            cameraShake.Call(shakeTime, shakeRange);
            Weapon_Explosion tempExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
            tempExplosion.OnTargetKill += OnTargetKill;
            tempExplosion.damage = damage;
            tempExplosion.damageMultiplier = damageMultiplier;

            Destroy(gameObject);
        }
            
    }

    private void OnMissileFired()
    {
        ParticleSystem trail = Instantiate(smokeTrail, transform.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trail));

        _rb.AddForce(forward * speed, ForceMode.Impulse);
        missileFired = true;
    }

    IEnumerator SpawnTrail(ParticleSystem trail)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, transform.position, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        trail.transform.position = transform.position;

        Destroy(trail.gameObject, trail.time);
    }
}