using UnityEngine;

public class ExampleWeapon1 : MonoBehaviour, IFireable
{
    public float baseCooldown;

    public void Fire()
    {
        print(name + " fires");
    }
    
    public float GetBaseCooldown()
    {
        return baseCooldown;
    }
}
