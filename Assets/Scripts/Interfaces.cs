public interface IFireable
{
    void Fire();
    float GetBaseCooldown();
}

public interface IDamageable
{
    void Die();
    void ReactToDamage();
}