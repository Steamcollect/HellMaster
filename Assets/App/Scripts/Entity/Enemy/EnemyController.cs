using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] RSO_PlayerTransform rsoTarget;
    [SerializeField] WeaponTemplate weapon;

    Transform lookAtRot;

    //[Space(10)]
    // RSO
    // RSF
    // RSP

    //[Header("Input")]
    //[Header("Output")]

    private void Start()
    {
        lookAtRot = new GameObject("LookAtRot").transform;
        lookAtRot.parent = transform;
    }

    private void Update()
    {
        agent.destination = rsoTarget.Value.position;

        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            lookAtRot.position = transform.position;
            lookAtRot.LookAt(rsoTarget.Value);
            weapon.OnAttack(transform.position, lookAtRot.forward);
        }
    }
}