using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Allegiance allegiance;

    [HideInInspector] public float damage;
    string tagToDamage;

    public enum Allegiance { Friend, Foe }

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

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(tagToDamage))
        {
            collider.GetComponent<IDamageable>().Damage(damage);
        }

        Destroy(gameObject);
    }
}
