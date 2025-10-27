using UnityEngine;

public class Health : MonoBehaviour
{
    public int startHealth;
    public ProgressBar healthBar;
    public MonoBehaviour controllerScript;
    public GameEvent onDeath; //Used to broadcast publicly


    int health;
    IDamageable damageable;

    void Start()
    {
        damageable = controllerScript.GetComponent<IDamageable>();

        health = startHealth;

        if (healthBar != null)
            healthBar.SetMaxValue(startHealth);
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
}
