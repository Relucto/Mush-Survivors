using UnityEngine;
using UnityEngine.AI;

public class TestFollowPlayer : MonoBehaviour
{
    public float attackRange;

    NavMeshAgent agent;
    Transform playerT;
    Animator anim;

    void Start()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.destination = playerT.position;

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        agent.destination = playerT.position;

        if (agent.remainingDistance < attackRange)
        {
            anim.SetBool("Attacking", true);
        }
        else
        {
            anim.SetBool("Attacking", false);
        }
    }
}
