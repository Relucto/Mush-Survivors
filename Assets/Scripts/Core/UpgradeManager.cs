using UnityEngine;

public class UpgradeManager : MonoBehaviour, IAwaitable
{
    public PlayerUpgrade healthUpgrade, speedUpgrade;

    bool isReady;

    void Awake()
    {
        healthUpgrade.SetLevel(1);
        speedUpgrade.SetLevel(1);
    }

    void Start()
    {
        isReady = true;
    }

    public bool IsReady() => isReady;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            LevelUpStat(healthUpgrade);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            LevelUpStat(speedUpgrade);
        }
    }

    public void LevelUpStat(PlayerUpgrade upgrade)
    {
        if (upgrade.IsMaxLevel())
            return;

        upgrade.IncreaseLevel();

        print(upgrade.name + " lvl " + upgrade.GetLevel() + ". Value: " + upgrade.GetLevelValue());
    }
}
