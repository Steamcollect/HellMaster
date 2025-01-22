using UnityEngine;

public class FlyingEnemyController : MonoBehaviour, IResettable
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] Collider enemyCollider;
    [SerializeField] RSO_PlayerPosition rsoTarget;
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

    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        if(!isDead)
        {
            
            lookAtRot.position = transform.position;
            lookAtRot.LookAt(rsoTarget.Value);
            this.gameObject.transform.rotation = lookAtRot.rotation;

            if (Vector3.Distance(transform.position, rsoTarget.Value) < attackDistance)
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
        transform.position = Vector3.MoveTowards(transform.position, rsoTarget.Value, speed * Time.deltaTime);
    }

    public void ResetState()
    {
        isDead = false;
        enemyCollider.enabled = true;
    }
}