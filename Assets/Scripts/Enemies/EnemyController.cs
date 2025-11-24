using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IEntity
{
    [Header("Stats")]
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
    public NavMeshAgent agent;
    public Rigidbody rb;

    [HideInInspector] public Transform playerT;
    Vector3 LookAtTarget;
    float currentCooldown, currentStunDuration, distance;
    
    bool isDead, isStunned, isMoving;

    void Start()
    {
        isStunned = false;
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

        if (!agent.isActiveAndEnabled)
            return;

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
        if (distance <= agent.stoppingDistance + 0.1f)
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

            EnemySpawner.numEnemies--;

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
