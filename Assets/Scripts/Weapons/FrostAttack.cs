using System.Collections.Generic;
using UnityEngine;
using ElementalEffects;
using AudioSystem;

public class FrostAttack : MonoBehaviour
{
    public PlayerUpgrade frostStats;
    public ParticleSystem[] particles;
    public AudioPair frostSound;
    public AudioChannel sfx;

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

        foreach (ParticleSystem particle in particles)
        {
            particle.Play();
        }

        sfx.Play(frostSound.clip, frostSound.volume, frostSound.pitchVariance, transform);
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
        if (collider.CompareTag("Enemy") || collider.CompareTag("Plant"))
        {
            if (alreadyDamaged.Contains(collider))
                return;
            
            float critDamage = WeaponHandler.CritDamage();
            bool isCritical = critDamage == 0 ? false : true;

            collider.GetComponent<IDamageable>().Damage(damage + critDamage, isCritical, DamageType.ice, false);
            alreadyDamaged.Add(collider);
        }
    }
}
