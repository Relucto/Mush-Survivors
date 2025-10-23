using UnityEngine;

public class ExampleWeapon2 : MonoBehaviour, IFireable
{
    public void Fire()
    {
        print("Hi I'm also firing a weapon, my name is " + name + " :)");
    }
}
