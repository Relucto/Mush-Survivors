using UnityEngine;

public class ExampleWeapon1 : MonoBehaviour, IFireable
{
    
    
    public void Fire()
    {
        print(name + " fires");
    }
}
