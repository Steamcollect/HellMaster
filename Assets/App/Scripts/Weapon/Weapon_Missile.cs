using System;
using UnityEngine;
public class Weapon_Missile : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] private float speed = 10f;
    //private GameObject smokeTrail;
    //[SerializeField]
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
        if (!other.CompareTag("Player"))
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
        //smokeTrail.SetActive(true);
        _rb.AddForce(forward * speed, ForceMode.Impulse);
        missileFired = true;
    }
}