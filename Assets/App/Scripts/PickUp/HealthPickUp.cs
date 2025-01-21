using System.Collections;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float recorveryDelay;
    [SerializeField] GameObject visual;
    [SerializeField] int healthGiven;
    [SerializeField] Collider collid;

    bool canPick = true;
    //[Header("References")]

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    //[Header("Input")]
    //[Header("Output")]

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out IHealth health) && canPick)
        {
            health.Heal(healthGiven);
            StartCoroutine(RecorveryCooldown());
        }
    }

    IEnumerator RecorveryCooldown()
    {
        canPick = false;
        visual.SetActive(false);
        collid.enabled = false;
        yield return new WaitForSeconds(recorveryDelay);
        collid.enabled = true;
        visual.SetActive(true);
        canPick = true;
    }
}