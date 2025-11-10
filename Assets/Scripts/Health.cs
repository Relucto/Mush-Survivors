using UnityEngine;

public class Health : MonoBehaviour, IAwaitable, IDamageable
{
    public int maxHealth;
    public ProgressBar healthBar;
    public MonoBehaviour controllerScript;
    public GameEvent onDeath; //Used to broadcast publicly
    public PlayerUpgrade healthStat;

    int health;
    IEntity entity;

    bool isReady;
    public bool IsReady() => isReady;

    void OnEnable()
    {
        if (healthStat != null)
        {
            healthStat.levelUp += IncreaseOnLevelUp;
        }
    }

    void OnDisable()
    {
        if (healthStat != null)
        {
            healthStat.levelUp -= IncreaseOnLevelUp;
        }
    }

    void Start()
    {
        entity = controllerScript.GetComponent<IEntity>();

        if (healthStat != null)
            SetMaxHealth(healthStat.GetLevelValue().stats[0].value);

        health = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetMaxValue(maxHealth);
            healthBar.SetValue(maxHealth);
        }

        isReady = true;
    }

    //TESTING!!!!!!!!!!!!!!!!!!!!!!!!!1
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Damage(10);
        }
    }

    public void Damage(float damage)
    {
        if (health <= 0)
            return;

        health -= (int)damage;

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

    void IncreaseOnLevelUp()
    {
        PlayerUpgrade.LevelStatGroup statGroup = healthStat.GetLevelValue();

        if (statGroup.stats.Length != 1)
        {
            Debug.LogError(healthStat.name + " has incorrect number of values");
            Debug.Break();
            return;
        }

        SetMaxHealth(statGroup.stats[0].value);
        print("Max health is now " + maxHealth);
    }
}
