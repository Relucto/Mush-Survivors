using UnityEngine;

public class SporeBulletLauncher : MonoBehaviour
{
    public PlayerUpgrade sporeBulletStats;
    public GameObject projectilePrefab;
    public Transform spawnPoint; // Rotates with animation
    public Animator spinAnim;


    [Header("Pool settings")]
    public Transform poolParent;
    public int startSize, maxSize;

    Pool pool;
    float damage; // [0]
    float cooldown; // [1]
    float currentCooldown;
    bool isActive = false;

    void OnEnable()
    {
        sporeBulletStats.levelUp += SetStats;
        sporeBulletStats.requestActivation += ActivateSelf;
    }

    void OnDisable()
    {
        sporeBulletStats.levelUp -= SetStats;
        sporeBulletStats.requestActivation -= ActivateSelf;
    }

    void ActivateSelf()
    {
        isActive = true;
        spinAnim.enabled = true;
    }

    void Start()
    {
        SetStats();

        pool = new Pool(projectilePrefab, startSize, maxSize, poolParent);

        spinAnim.enabled = false;
        
    }

    void Update()
    {
        if (!isActive)
            return;

        if (currentCooldown < 0)
        {
            currentCooldown = cooldown;
            
            FireBullet();
        }

        currentCooldown -= Time.deltaTime;
    }

    void FireBullet()
    {
        // Grab from pool
        GameObject bullet = pool.Get();

        bullet.transform.position = spawnPoint.position;
        //GameObject bullet = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);

        // Calculate move direction
        Vector3 pos1 = transform.position;
        Vector3 pos2 = spawnPoint.position;
        pos1.y = 0;
        pos2.y = 0;
        
        Vector3 moveDirection = pos2 - pos1;

        // Set data
        SporeBullet sporeBullet = bullet.GetComponent<SporeBullet>();
        sporeBullet.direction = moveDirection;
        sporeBullet.pool = pool;

        Projectile projectile = bullet.GetComponent<Projectile>();
        projectile.damage = damage;
    }

    void SetStats()
    {
        PlayerUpgrade.LevelStatGroup statGroup = sporeBulletStats.GetLevelValue();

        damage = statGroup.stats[0].value;
        cooldown = statGroup.stats[1].value;
        spinAnim.speed = statGroup.stats[2].value;

        currentCooldown = cooldown;
    }
}
