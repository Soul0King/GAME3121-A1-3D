using UnityEngine;
using UnityEngine.AI;

public class EnemyControllr : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        animator = GetComponentInChildren<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }

        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized) * Quaternion.Euler(0, 90f, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, (targetRotation), Time.deltaTime * 10f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.name == "Player")
        {
            animator.SetTrigger("HammerSmash");
        }
        
    }


}
