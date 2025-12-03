using UnityEngine;
using ElementalEffects;
using AudioSystem;

public class Health : MonoBehaviour, IAwaitable, IDamageable
{
    public int maxHealth;
    public ProgressBar healthBar;
    public float damageIndicatorYOffset = 1;
    public MonoBehaviour controllerScript;
    public GameEvent onDeath; //Used to broadcast publicly
    public PlayerUpgrade healthStat, armorStat, frostStats;
    public bool hasRegen;
    public int regenRate;

    [Header("Audio - optional")]
    public AudioChannel sfx;
    public AudioPair deathSound;
    public AudioSource hitSource;

    [Header("Player Materials")]
    public SkinnedMeshRenderer[] renderers;
    public Material normalMat;
    public Material redMat;
    public float redTime;

    [Header("Elemental Effects")]
    public float burnDamage;
    public float burnTime;
    public float burnDamageCooldown;
    public float slowTime;

    float currentRedTime, regenCooldown;
    float currentBurnTime, currentBurnDamageCooldown, currentSlowTime, slowSpeedMult;
    float health;
    float armor;
    IEntity entity;
    Vector3 spawnIndicatorOffset, damageIndicatorSpawn;

    bool isReady, burning, freezing;
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
        if (frostStats != null)
        {
            frostStats.levelUp += SetSlow;
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
        if (frostStats != null)
        {
            frostStats.levelUp -= SetSlow;
        }
    }

    void Start()
    {
        entity = controllerScript.GetComponent<IEntity>();

        armor = 0;

        if (healthStat != null)
        {
            healthStat.SetLevel(1);
            SetMaxHealth(healthStat.GetLevelValue().stats[0].value);
        }
            
        if (armorStat != null) 
        {
            armorStat.SetLevel(1);
            SetArmor(armorStat.GetLevelValue().stats[0].value);
        }

        if (frostStats != null)
        {
            SetSlow();
        }

        health = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetMaxValue(maxHealth);
            healthBar.SetValue(maxHealth);
        }

        spawnIndicatorOffset.y = damageIndicatorYOffset;

        isReady = true;
    }

    void Update()
    {
        if (currentBurnTime > 0)
        {
            currentBurnTime -= Time.deltaTime;
            currentBurnDamageCooldown -= Time.deltaTime;

            if (currentBurnDamageCooldown <= 0)
            {
                currentBurnDamageCooldown = burnDamageCooldown;

                Damage(maxHealth * 0.03f, false, DamageType.physical, false);
            }
        }
        if (currentSlowTime > 0)
        {
            currentSlowTime -= Time.deltaTime;
            
            if (currentSlowTime <= 0)
            {
                entity.NormalSpeed();
            }
        }
        if (currentRedTime > 0)
        {
            currentRedTime -= Time.deltaTime;

            if (currentRedTime <= 0)
            {
                foreach (SkinnedMeshRenderer renderer in renderers)
                {
                    renderer.material = normalMat;
                }
            }
        }

        if (hasRegen) // Player
        {
            regenCooldown -= Time.deltaTime;

            if (regenCooldown <= 0)
            {
                regenCooldown = 1;
                Heal(regenRate);
            }
        }
    }
    
    public void Damage(float damage, bool isCritical, DamageType damageType, bool reactToDamage)
    {
        if (health <= 0)
            return;
        
        float finalDamage = damage * (1 - armor);

        health -= finalDamage;

        if (hitSource != null)
        {
            /*
            if (!hitSource.isPlaying)
            {
                
            } */
            hitSource.pitch = 1 + Random.Range(-0.35f, 0.35f);
            hitSource.Play();
        }

        // Spawn damage indicator (on enemy)
        if (gameObject.CompareTag("Player") == false)
        {
            damageIndicatorSpawn = healthBar.transform.position;

            // Spawn damage indicator
            DamageIndicatorManager.Instance.SpawnIndicator(damageIndicatorSpawn + spawnIndicatorOffset, finalDamage, isCritical);
        }
        else // If this is the player, make them red
        {
            foreach (SkinnedMeshRenderer renderer in renderers)
            {
                renderer.material = redMat;
            }

            currentRedTime = redTime; // Start countdown to return to normal
        }
        
        if (reactToDamage)
            entity.ReactToDamage();

        if (health <= 0)
        {
            health = 0;

            if (deathSound.clip != null)
            {
                sfx.Play(deathSound.clip, deathSound.volume, deathSound.pitchVariance, transform.position);
            }

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

        switch (damageType)
        {
            case DamageType.fire:
                    ApplyBurn();
                break;
            
            case DamageType.ice:
                    ApplySlow();
                break;
        }
    }

    void ApplyBurn()
    {
        currentBurnTime = burnTime;
        currentBurnDamageCooldown = burnDamageCooldown;
    }

    void ApplySlow()
    {
        currentSlowTime = slowTime;

        entity.SlowSpeed(slowSpeedMult);
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

    public void SetMaxHealth(float value)
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

        int temp = maxHealth;

        SetMaxHealth(statGroup.stats[0].value);

        //Heal new health back up
        Heal(maxHealth - temp);
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

    void SetSlow()
    {
        PlayerUpgrade.LevelStatGroup statGroup = frostStats.GetLevelValue();

        slowSpeedMult = statGroup.stats[2].value;
    }
}
