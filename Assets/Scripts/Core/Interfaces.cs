using ElementalEffects;

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
    void Damage(float value, bool isCritical, DamageType damageType, bool reactToDamage);
}

public interface IAwaitable
{
    bool IsReady();
}

namespace ElementalEffects
{
    public enum DamageType {physical, fire, ice}
}