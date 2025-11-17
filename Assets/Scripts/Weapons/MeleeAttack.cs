using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class MeleeAttack : MonoBehaviour
{
    public PlayerUpgrade weaponStats;

    Collider[] colliders;

    float damage;

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
    }

    public void EndAttack()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            collider.GetComponent<IDamageable>().Damage(damage);
        }
    }
}
