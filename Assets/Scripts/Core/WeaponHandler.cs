using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public PlayerUpgrade critChanceStats, critDamageStats;
    
    static float critChance, critDamage;

    void OnEnable()
    {
        critChanceStats.levelUp += LevelUpCritChance;
        critDamageStats.levelUp += LevelUpCritDamage;
    }

    void OnDisable()
    {
        critChanceStats.levelUp -= LevelUpCritChance;
        critDamageStats.levelUp -= LevelUpCritDamage;
    }

    void Start()
    {
        critChanceStats.SetLevel(1);
        critDamageStats.SetLevel(1);
        
        SetCritChance(critChanceStats.GetLevelValue().stats[0].value);
        SetCritDamage(critDamageStats.GetLevelValue().stats[0].value);
    }

    void SetCritChance(float value) => critChance = value;

    void SetCritDamage(float value) => critDamage = value;

    void LevelUpCritChance()
    {
        PlayerUpgrade.LevelStatGroup statGroup = critChanceStats.GetLevelValue();
        
        if (statGroup.stats.Length != 1)
        {
            Debug.LogError(critChanceStats.name + " has incorrect number of values");
            Debug.Break();
            return;
        }

        SetCritChance(statGroup.stats[0].value);
    }

    void LevelUpCritDamage()
    {
        PlayerUpgrade.LevelStatGroup statGroup = critDamageStats.GetLevelValue();
        
        if (statGroup.stats.Length != 1)
        {
            Debug.LogError(critDamageStats.name + " has incorrect number of values");
            Debug.Break();
            return;
        }

        SetCritDamage(statGroup.stats[0].value);
    }

    public static float CritDamage()
    {
        return Random.Range(1, 101) <= critChance ? critDamage : 0;
    }
}
