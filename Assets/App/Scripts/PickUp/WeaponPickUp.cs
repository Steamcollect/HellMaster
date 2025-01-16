using System.Xml.Serialization;
using TMPro;
using UnityEngine;
public class WeaponPickUp : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int achievmentCountRequire;

    [Header("References")]
    [SerializeField] WeaponTemplate weaponPrefab;
    [SerializeField] TMP_Text achivementCountTxt;

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
        rsoAchievmentCompleteCount.OnChanged += UpdateTxtVisual;
    }
    private void OnDisable()
    {
        rseOnGameStart.action -= LateStart;
        rsoAchievmentCompleteCount.OnChanged -= UpdateTxtVisual;
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

    void UpdateTxtVisual()
    {
        achivementCountTxt.text = rsoAchievmentCompleteCount.Value.ToString() + "/" + achievmentCountRequire;
    }
}