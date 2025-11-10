public interface IFireable
{
    void Fire();
    float GetBaseCooldown();
}

public interface IEntity
{
    void Die();
    void ReactToDamage();
}

public interface IDamageable
{
    void Damage(float value);
}

public interface IAwaitable
{
    bool IsReady();
}