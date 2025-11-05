using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class W_Vine : MonoBehaviour
{
    public PlayerUpgrade weaponStats;
    public string attackAnimation;

    Animator animator;

    float damage; // weaponStats [0]
    float cooldown; // weaponStats [1]
    float currentCooldown;

    void Start()
    {
        animator = GetComponent<Animator>();

        damage = weaponStats.GetLevelValue().stats[0].value;
        cooldown = weaponStats.GetLevelValue().stats[1].value;
    }

    public void Fire()
    {
        animator.SetTrigger("Attacking");

        currentCooldown = cooldown;
    }

    public void DealDamage()
    {

    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation))
        {
            print("Attacking...");
        }
        else
        {
            currentCooldown -= Time.deltaTime;

            if (currentCooldown <= 0)
            {
                Fire();
            }
        }
    }
}
