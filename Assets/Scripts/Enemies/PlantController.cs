using UnityEngine;

public class PlantController : MonoBehaviour, IEntity
{
    public Animator anim;
    public float attackCooldown;
    public Transform projSpawnLocation;
    public GameObject projectilePrefab;
    public float attackDamage;
    public float xpValue;
    public GameObject xpPrefab;
    
    [HideInInspector] public Transform playerT;

    float currentCooldown;
    AnimatorStateInfo state;
    bool isDead;
    
    void Update()
    {
        state = anim.GetCurrentAnimatorStateInfo(0);

        if (state.IsName("Plant Enter") == true)
            return;

        // Look at player
        Vector3 lookPos = playerT.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;

        // Attack / Cooldown
        if (currentCooldown < 0)
        {
            currentCooldown = attackCooldown;
            ShootProjectile();
        }
        else if (state.IsName("Attacking") == false)
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    void ShootProjectile()
    {
        anim.SetTrigger("Attacking");
        GameObject obj = Instantiate(projectilePrefab, projSpawnLocation.position, Quaternion.identity);
        PlantProjectile projectile = obj.GetComponent<PlantProjectile>();

        // Set info here
        projectile.direction = (playerT.position - projSpawnLocation.position).normalized;
        projectile.damage = attackDamage;
    }

    public void Die()
    {
        if (isDead == false)
        {
            isDead = true;

            EnemySpawner.numEnemies--;

            // Spawn xp
            Instantiate(xpPrefab, transform.position, transform.rotation).GetComponent<XPOrb>().SetXPValue(xpValue);

            EnemySpawner.Instance.PlantKilled(transform.position);

            Destroy(gameObject);
        }
    }

    public void ReactToDamage()
    {
        
    }

    public void NormalSpeed()
    {
        
    }

    public void SlowSpeed(float value)
    {
        
    }
}
