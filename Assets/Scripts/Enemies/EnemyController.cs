using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IEntity
{
    [Header("Stats")]
    public float attackRange;
    public float attackCooldown;
    public float knockbackForce = 5;
    public float stunDuration;
    public float xpValue = 10;

    [Header("Priority Settings")]
    public float farthestPriorityDistance = 50;

    [Header("Cache")]
    public Animator anim;
    public Material material;
    public GameObject xpPrefab;

    Transform playerT;
    NavMeshAgent agent;
    Rigidbody rb;
    Vector3 LookAtTarget;
    float currentCooldown, currentStunDuration, distance;
    
    bool isDead, isStunned, isMoving;

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
            if (currentStunDuration < 0)
            {
                isStunned = false;
                rb.isKinematic = true;
                agent.Warp(transform.position);
                agent.enabled = true;
            }

            currentStunDuration -= Time.deltaTime;

            return;
        }

        agent.destination = playerT.position;

        distance = agent.remainingDistance;
        isMoving = distance > agent.stoppingDistance ? true : false;

        if (isMoving) // Adjust priority
        {
            // Set priority. Closer = higher priority
            agent.avoidancePriority = (int)Remap(distance, agent.stoppingDistance, farthestPriorityDistance, 1, 300);
        }
        else // Look at player
        {
            Vector3 lookPos = agent.destination - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = rotation;
        }

        // ==================
        // Animation

        anim.SetBool("Moving", isMoving);

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

    float Remap(float s, float a1, float a2, float b1, float b2) 
    { 
        return b1 + (s-a1)*(b2-b1)/(a2-a1); 
    }
}
