using UnityEngine;
public class DamageHaloUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator animator;

    [Header("Input")]
    [SerializeField] RSE_OnPlayerHit rseOnPlayerHit;

    private void OnEnable()
    {
        rseOnPlayerHit.action += DamageAnim;
    }

    private void OnDisable()
    {
        rseOnPlayerHit.action -= DamageAnim;
    }

    void DamageAnim()
    {
        animator.SetTrigger("Damage");
    }
}