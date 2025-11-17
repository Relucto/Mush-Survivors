using UnityEngine;

public class SporeBullet : MonoBehaviour
{
    public Projectile projectileScript;
    public float moveSpeed;
    
    //[HideInInspector] public float damage;
    [HideInInspector] public Vector3 direction;

    void Update()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
}
