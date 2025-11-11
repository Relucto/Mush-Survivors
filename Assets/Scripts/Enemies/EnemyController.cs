using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements.Experimental;

public class EnemyController : MonoBehaviour, IEntity
{
    public float attackRange;
    public GameObject xpPrefab;
    public float xpValue = 10;

    Transform playerT;
    NavMeshAgent agent;
    Animator anim;

    bool isDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = playerT.position;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            anim.SetBool("Attacking", true);
        }
        else
        {
            anim.SetBool("Attacking", false);
        }
    }

    public void ReactToDamage()
    {
        if (isDead)
            return;
            
        //React however needed
    }

    public void Die()
    {
        if (isDead == false)
        {
            isDead = true;

            // Spawn xp
            Instantiate(xpPrefab, transform.position, transform.rotation).GetComponent<XPOrb>().SetXPValue(xpValue);

            Destroy(gameObject);
        }
    }
}
