using System.Collections.Generic;
using UnityEngine;
using ElementalEffects;

public class FrostAttack : MonoBehaviour
{
    public PlayerUpgrade frostStats;

    Collider[] colliders;
    List<Collider> alreadyDamaged = new List<Collider>();

    float damage, slowMult;
    bool isActive;

    void OnEnable()
    {
        frostStats.levelUp += SetStats;
    }

    void OnDisable()
    {
        frostStats.levelUp -= SetStats;
    }

    void SetStats()
    {
        PlayerUpgrade.LevelStatGroup statGroup = frostStats.GetLevelValue();

        damage = statGroup.stats[0].value;
    }

    void Start()
    {
        frostStats.SetLevel(1);

        SetStats();

        colliders = GetComponentsInChildren<Collider>();
        print(colliders.Length);

        // Disable weapon colliers for now. They are enabled in animation
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }

    public void StartAttack()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }

    public void EndAttack()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
        
        alreadyDamaged.Clear();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            if (alreadyDamaged.Contains(collider))
                return;
            
            float critDamage = WeaponHandler.CritDamage();
            bool isCritical = critDamage == 0 ? false : true;

            collider.GetComponent<IDamageable>().Damage(damage + critDamage, isCritical, DamageType.ice, true);
            alreadyDamaged.Add(collider);
        }
    }
}
