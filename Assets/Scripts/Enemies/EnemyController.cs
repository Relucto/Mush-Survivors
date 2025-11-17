using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IEntity
{
    public float attackRange;
    public float attackCooldown;
    public GameObject xpPrefab;
    public float xpValue = 10;
    public Animator anim;
    public Material material;

    Transform playerT;
    NavMeshAgent agent;
    float currentCooldown;
    
    bool isDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = playerT.position;

        float distance = agent.remainingDistance;

        // ==================
        // Movement Animation

        anim.SetBool("Moving", distance > agent.stoppingDistance ? true : false);

        // ==================
        // Attack Animation

        // If within attack range
        if (distance <= attackRange)
        {
            // If off of cooldown, attack
            if (currentCooldown < 0)
            {
                anim.SetTrigger("Attacking");
                currentCooldown = attackCooldown;
            }
        }

        // Reduce cooldown if not attacking
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attacking") == false)
        {
            currentCooldown -= Time.deltaTime;
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
