using UnityEngine;

public class SporeBullet : MonoBehaviour
{
    public Projectile projectileScript;
    public float moveSpeed;
    public float lifeTime;
    
    //[HideInInspector] public float damage;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public Pool pool;

    float currentLifeTime;

    void OnEnable()
    {
        currentLifeTime = lifeTime;
    }

    void Update()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        if (currentLifeTime < 0)
        {
            pool.Return(gameObject);
        }

        currentLifeTime -= Time.deltaTime;
    }
}
