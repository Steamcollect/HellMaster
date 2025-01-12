using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemyController : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] Collider enemyCollider;
    [SerializeField] RSO_PlayerTransform rsoTarget;
    [SerializeField] WeaponTemplate weapon;
    [SerializeField] float speed;
    [SerializeField] float attackDistance;
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
            
            lookAtRot.position = transform.position;
            lookAtRot.LookAt(rsoTarget.Value.position);
            this.gameObject.transform.rotation = lookAtRot.rotation;

            if (Vector3.Distance(transform.position, rsoTarget.Value.position) < attackDistance)
            {
                animator.SetTrigger("Attack");
                weapon.OnAttack(transform.position, lookAtRot.forward);
            }
            else
            {
                Chase();
                animator.SetBool("isRunning", true);
            }
        }
        else { enemyCollider.enabled = false; }
    }

    void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, rsoTarget.Value.position, speed * Time.deltaTime);
    }
}