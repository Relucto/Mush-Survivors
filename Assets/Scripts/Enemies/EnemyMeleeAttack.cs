using UnityEngine;
using ElementalEffects;

public class EnemyMeleeAttack : MonoBehaviour
{
    public float attackDamage;
    public SphereCollider attackCollider;

    bool hitPlayer;

    void Start()
    {
        attackCollider.enabled = false;
        hitPlayer = false;
    } 

    public void StartAttack()
    {
        hitPlayer = false;
        attackCollider.enabled = true;
    } 

    public void EndAttack()
    {
        attackCollider.enabled = false;
    } 

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if (hitPlayer)
                return;
            
            hitPlayer = true;

            collider.GetComponent<Health>().Damage(attackDamage, false, DamageType.physical, true);
        }
    }
}
