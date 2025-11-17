using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Allegiance allegiance;
    public float lifetime;

    [HideInInspector] public float damage;
    [HideInInspector] public Pool pool;
    string tagToDamage;
    float currentLifetime;

    public enum Allegiance { Friend, Foe }

    bool returned = false;

    void OnEnable()
    {
        returned = false;

        currentLifetime = lifetime;
    }

    void Start()
    {
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
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(tagToDamage))
        {
            collider.GetComponent<IDamageable>().Damage(damage);
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
