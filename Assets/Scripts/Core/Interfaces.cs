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
    void Damage(float value, bool isCritical);
}

public interface IAwaitable
{
    bool IsReady();
}

public interface IUpdateManageable
{
    void ManagedUpdate();
}