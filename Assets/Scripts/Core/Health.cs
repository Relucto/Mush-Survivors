using UnityEngine;

public class Health : MonoBehaviour, IAwaitable, IDamageable
{
    public int maxHealth;
    public ProgressBar healthBar;
    public float damageIndicatorYOffset = 1;
    public MonoBehaviour controllerScript;
    public GameEvent onDeath; //Used to broadcast publicly
    public PlayerUpgrade healthStat, armorStat;

    float health;
    float armor;
    IEntity entity;
    Vector3 spawnIndicatorOffset, damageIndicatorSpawn;

    bool isReady;
    public bool IsReady() => isReady;

    void OnEnable()
    {
        if (healthStat != null)
        {
            healthStat.levelUp += LevelUpHealth;
        }
        if (armorStat != null)
        {
            armorStat.levelUp += LevelUpArmor;
        }
    }

    void OnDisable()
    {
        if (healthStat != null)
        {
            healthStat.levelUp -= LevelUpHealth;
        }
        if (armorStat != null)
        {
            armorStat.levelUp -= LevelUpArmor;
        }
    }

    void Start()
    {
        entity = controllerScript.GetComponent<IEntity>();

        armor = 0;

        if (healthStat != null)
            SetMaxHealth(healthStat.GetLevelValue().stats[0].value);
        if (armorStat != null)
            SetArmor(armorStat.GetLevelValue().stats[0].value);

        health = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetMaxValue(maxHealth);
            healthBar.SetValue(maxHealth);
        }

        spawnIndicatorOffset.y = damageIndicatorYOffset;

        isReady = true;
    }

    public void Damage(float damage)
    {
        if (health <= 0)
            return;
        
        float finalDamage = damage * (1 - armor);

        health -= finalDamage;

        damageIndicatorSpawn = gameObject.CompareTag("Player") ? transform.position : healthBar.transform.position;

        // Spawn damage indicator
        DamageIndicatorManager.Instance.SpawnIndicator(damageIndicatorSpawn + spawnIndicatorOffset, finalDamage);

        entity.ReactToDamage();

        if (health <= 0)
        {
            health = 0;

            //Raise death events
            if (onDeath != null)
            {
                onDeath.Raise();
            }
            
            //React to death event
            entity.Die();
        }

        //Set health bar
        if (healthBar != null)
            healthBar.SetValue(health);
    }

    public void Heal(int amount)
    {
        //If already dead, do nothing
        if (health <= 0)
            return;

        health += amount;

        //Clamp
        if (health >= maxHealth)
            health = maxHealth;

        //Update health bar
        if (healthBar != null)
            healthBar.SetValue(health);
    }

    void SetMaxHealth(float value)
    {
        maxHealth = (int)value;
        healthBar.SetMaxValue(value);
    }

    void SetArmor(float value)
    {
        armor = value;
    }

    void LevelUpHealth()
    {
        PlayerUpgrade.LevelStatGroup statGroup = healthStat.GetLevelValue();

        if (statGroup.stats.Length != 1)
        {
            Debug.LogError(healthStat.name + " has incorrect number of values");
            Debug.Break();
            return;
        }

        SetMaxHealth(statGroup.stats[0].value);
    }

    void LevelUpArmor()
    {
        PlayerUpgrade.LevelStatGroup statGroup = armorStat.GetLevelValue();

        if (statGroup.stats.Length != 1)
        {
            Debug.LogError(armorStat.name + " has incorrect number of values");
            Debug.Break();
            return;
        }

        SetArmor(statGroup.stats[0].value);
    }
}
