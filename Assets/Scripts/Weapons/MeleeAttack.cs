using System.Collections.Generic;
using UnityEngine;
using ElementalEffects;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class MeleeAttack : MonoBehaviour
{
    public PlayerUpgrade weaponStats;
    public TrailRenderer trailRenderer;

    Collider[] colliders;
    float damage;
    List<Collider> alreadyDamaged = new List<Collider>();

    void OnEnable()
    {
        weaponStats.levelUp += SetDamage;
    }

    void OnDisable()
    {
        weaponStats.levelUp -= SetDamage;
    }

    void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
        trailRenderer.emitting = false;

        // Disable weapon colliers for now. They are enabled in animation
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb.isKinematic == false)
        {
            Debug.LogWarning("Rigidbody is not kinematic for " + gameObject.name);
            Debug.Break();
        }

        weaponStats.SetLevel(1);
        SetDamage();
    }

    public void SetDamage()
    {
        damage = weaponStats.GetLevelValue().stats[0].value; // [0] is always damage
    }

    public void StartAttack()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
        trailRenderer.emitting = true;
    }

    public void EndAttack()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
        trailRenderer.emitting = false;
        
        alreadyDamaged.Clear();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy") || collider.gameObject.CompareTag("Plant"))
        {
            if (alreadyDamaged.Contains(collider))
                return;
            
            float critDamage = WeaponHandler.CritDamage();
            bool isCritical = critDamage == 0 ? false : true;

            collider.GetComponent<IDamageable>().Damage(damage + critDamage, isCritical, DamageType.physical, true);
            alreadyDamaged.Add(collider);
        }
    }
}
