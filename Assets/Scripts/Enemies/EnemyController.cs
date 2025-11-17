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
    public float knockbackForce = 5;
    public float stunDuration;

    Transform playerT;
    NavMeshAgent agent;
    Rigidbody rb;
    float currentCooldown, currentStunDuration;
    
    bool isDead, isStunned;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStunned)
        {
            if (stunDuration < 0)
            {
                isStunned = false;
                rb.isKinematic = true;
                agent.Warp(transform.position);
                agent.enabled = true;
            }

            stunDuration -= Time.deltaTime;

            return;
        }

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
        if (isDead || isStunned)
            return;
            
        //React however needed
        isStunned = true;
        agent.enabled = false;
        rb.isKinematic = false;

        Vector3 direction = transform.position - playerT.position;

        rb.AddForce(direction * knockbackForce, ForceMode.Impulse);

        // Wait
        currentStunDuration = stunDuration;
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
