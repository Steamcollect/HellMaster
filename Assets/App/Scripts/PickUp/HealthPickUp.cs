using System.Collections;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float recorveryDelay;
    [SerializeField] int healthGiven;

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
        yield return new WaitForSeconds(recorveryDelay);
        canPick = true;
    }
}