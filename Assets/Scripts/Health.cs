using UnityEngine;

public class Health : MonoBehaviour, IAwaitable
{
    public int maxHealth;
    public ProgressBar healthBar;
    public MonoBehaviour controllerScript;
    public GameEvent onDeath; //Used to broadcast publicly
    public PlayerUpgrade healthStat;

    int health;
    IDamageable damageable;

    bool isReady;
    public bool IsReady() => isReady;

    void OnEnable()
    {
        if (healthStat != null)
        {
            healthStat.levelUp += SetMaxHealth;
        }
    }

    void OnDisable()
    {
        if (healthStat != null)
        {
            healthStat.levelUp -= SetMaxHealth;
        }
    }

    void Start()
    {
        damageable = controllerScript.GetComponent<IDamageable>();

        if (healthStat != null)
            SetMaxHealth(healthStat.GetLevelValue());

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

    public void Damage(int damage)
    {
        if (health <= 0)
            return;

        health -= damage;

        damageable.ReactToDamage();

        if (health <= 0)
        {
            health = 0;

            //Raise death events
            damageable.Die();

            if (onDeath != null)
            {
                onDeath.Raise();
            }
        }

        //Set health bar
        if (healthBar != null)
            healthBar.SetValue(health);
    }
    
    void SetMaxHealth(float value)
    {
        maxHealth = (int)value;
        healthBar.SetMaxValue(value);

        print("Max health is now " + maxHealth);
    }
}
