using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class Weapon : ScriptableObject
{
    public GameObject weaponPrefab;
    public int damage;
    public int attackCooldown;
}
