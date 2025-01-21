using UnityEngine;
public class HeallingHaloUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator animator;

    [Header("Input")]
    [SerializeField] RSE_OnHealth rseOnHealth;

    private void OnEnable()
    {
        rseOnHealth.action += HeallingAnim;
    }

    private void OnDisable()
    {
        rseOnHealth.action -= HeallingAnim;
    }

    void HeallingAnim(float health)
    {
        animator.SetTrigger("Healling");
    }
}