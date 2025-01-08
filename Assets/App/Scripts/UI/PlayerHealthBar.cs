using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthBar : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] Slider healthSlider;
    [SerializeField] TMP_Text healthPercentageTxt;

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    [Header("Input")]
    [SerializeField] RSE_UdateHealthBar rseUpdateHealthBar;

    //[Header("Output")]

    private void OnEnable()
    {
        rseUpdateHealthBar.action += UpdateHealthBar;
    }
    private void OnDisable()
    {
        rseUpdateHealthBar.action -= UpdateHealthBar;
    }

    void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        healthPercentageTxt.text = currentHealth / maxHealth * 100 + "%";
    }
}