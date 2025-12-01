using UnityEngine;

public class PlantProjectile : MonoBehaviour
{
    public float moveSpeed;
    public float lifeTime;

    [HideInInspector] public float damage;
    [HideInInspector] public Vector3 direction;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Enemy") || collider.gameObject.CompareTag("Plant"))
            return;

        if (collider.gameObject.CompareTag("Player"))
        {
            collider.GetComponent<IDamageable>().Damage(damage, false, ElementalEffects.DamageType.physical, true);
        }

        Destroy(gameObject);
    }
}
