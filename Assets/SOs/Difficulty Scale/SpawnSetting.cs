using UnityEngine;

[CreateAssetMenu(fileName = "SpawnSetting", menuName = "Scriptable Objects/SpawnSetting")]
public class SpawnSetting : ScriptableObject
{
    public float startTime;
    public float waitTime;
    public int maxEnemies;
    public EnemySpawner.EnemyType[] enemies;
}
