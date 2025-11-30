using UnityEngine;
using ElementalEffects;

public class Projectile : MonoBehaviour
{
    public float moveSpeed;
    public float lifetime;

    [HideInInspector] public Pool pool;
    [HideInInspector] public Allegiance allegiance;
    [HideInInspector] public Vector3 direction;
    float damage;
    string tagToDamage;
    float currentLifetime;

    public enum Allegiance { Friend, Foe }

    bool returned = false;

    void OnEnable()
    {
        returned = false;

        currentLifetime = lifetime;

        switch (allegiance)
        {
            case Allegiance.Friend:
                tagToDamage = "Enemy";
                break;
            case Allegiance.Foe:
                tagToDamage = "Player";
                break;
            default:
                Debug.LogError($"Projectile ({gameObject.name}) doesn't have a valid allegiance");
                Debug.Break();
                break;
        }
    }

    void Update()
    {
        if (currentLifetime < 0)
        {
            ReturnObject();
        }

        currentLifetime -= Time.deltaTime;

        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    public void SetDamage(float value)
    {
        damage = value;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(tagToDamage))
        {
            float critDamage = allegiance == Allegiance.Friend ? WeaponHandler.CritDamage() : 0;
            bool isCritical = critDamage == 0 ? false : true;

            collider.GetComponent<IDamageable>().Damage(damage + critDamage, isCritical, DamageType.physical, true);
            ReturnObject();
        }
    }

    void ReturnObject()
    {
        if (!returned)
        {
            returned = true;
            pool.Return(gameObject);
        }
    }
}
