using System.Xml.Serialization;
using UnityEngine;
public class WeaponPickUp : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int achievmentCountRequire;

    [Header("References")]
    [SerializeField] WeaponTemplate weaponPrefab;

    //[Space(10)]
    // RSO
    [SerializeField] RSO_AchievmentCompleteCount rsoAchievmentCompleteCount;
    // RSF
    // RSP

    [Header("Input")]
    [SerializeField] RSE_OnGameStart rseOnGameStart;
    [Header("Output")]
    [SerializeField] RSE_AddWeapon rseAddWeapon;

    private void OnEnable()
    {
        rseOnGameStart.action += LateStart;
    }
    private void OnDisable()
    {
        rseOnGameStart.action -= LateStart;
    }

    void LateStart()
    {
        Invoke("CheckCondition", .15f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            CheckCondition();
        }
    }

    void CheckCondition()
    {
        if(rsoAchievmentCompleteCount.Value >= achievmentCountRequire)
        {
            rseAddWeapon.Call(weaponPrefab);
            Destroy(gameObject);
        }
    }
}