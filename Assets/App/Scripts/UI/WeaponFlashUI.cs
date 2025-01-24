using UnityEngine;
public class WeaponFlashUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator animator;

    [Header("Input")]
    [SerializeField] RSE_OnShoot rseOnShoot;

    private void OnEnable()
    {
        rseOnShoot.action += FlashAnim;
    }

    private void OnDisable()
    {
        rseOnShoot.action -= FlashAnim;
    }

    void FlashAnim()
    {
        animator.SetTrigger("Flash");
    }
}