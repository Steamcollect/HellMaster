using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Collider enemyCollider;
    [SerializeField] RSO_PlayerTransform rsoTarget;
    [SerializeField] WeaponTemplate weapon;
    [SerializeField] Animator animator;
    public bool isDead = false;

    Transform lookAtRot;

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    //[Header("Input")]
    //[Header("Output")]

    private void Awake()
    {
        lookAtRot = new GameObject("LookAtRot").transform;
        lookAtRot.parent = transform;
    }

    private void Update()
    {
        if(!isDead)
        {
            agent.destination = rsoTarget.Value.position;
            animator.SetBool("isRunning", true);

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                animator.SetTrigger("Attack");
                lookAtRot.position = transform.position;
                lookAtRot.LookAt(rsoTarget.Value);
                weapon.OnAttack(transform.position, lookAtRot.forward);
            }
        }
        else { agent.isStopped = true; enemyCollider.enabled = false; }
    }
}