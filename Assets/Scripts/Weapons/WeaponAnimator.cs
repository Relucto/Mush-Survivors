using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    public PlayerUpgrade weaponStats;
    public Animator weaponAnimator;
    public bool printDebug;

    float cooldown; // weaponStats [1]
    float currentCooldown;

    bool isActive;

    void Awake()
    {
        isActive = false;
    }

    void OnEnable()
    {
        weaponStats.levelUp += SetStats;
        weaponStats.requestActivation += EnableSelf;
    }

    void OnDisable()
    {
        weaponStats.levelUp -= SetStats;
        weaponStats.requestActivation -= EnableSelf;
    }

    void Start()
    {
        SetStats();
    }

    void EnableSelf()
    {
        isActive = true;
    }

    void SetStats()
    {
        PlayerUpgrade.LevelStatGroup statGroup = weaponStats.GetLevelValue();

        cooldown = statGroup.stats[1].value;
    }

    public void Fire()
    {
        weaponAnimator.SetTrigger("Attacking");

        currentCooldown = cooldown;
    }

    void Update()
    {
        // This is the animator STATE. Not the animation clip itself
        if (weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (printDebug)
                print(gameObject.name + " is attacking");
        }
        else if (isActive)
        {
            currentCooldown -= Time.deltaTime;

            if (currentCooldown <= 0)
            {
                Fire();
            }
        }
    }
}
