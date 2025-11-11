using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour, IAwaitable
{
    [Header("Player Upgrades")]
    public PlayerUpgrade[] playerUpgrades;

    [Header("Weapons")]
    public int startWeaponIndex;
    public bool startWithoutWeapons;
    public PlayerUpgrade[] weaponUpgrades;
    

    PlayerUpgrade[] playerStats;

    [Header("Misc")]
    public GameObject upgradeScreen;
    public LevelUpChoice[] choiceOptions;
    public bool printDebug;

    List<PlayerUpgrade> possibleUpgrades = new List<PlayerUpgrade>();

    bool isReady;

    void Awake()
    {
        // Build playerStats array (both stat upgrades and weapon upgrades)
        playerStats = new PlayerUpgrade[playerUpgrades.Length + weaponUpgrades.Length];

        for (int i = 0; i < playerUpgrades.Length; i++)
        {
            playerStats[i] = playerUpgrades[i];
        }
        for (int i = playerUpgrades.Length, j = 0; i < playerUpgrades.Length + weaponUpgrades.Length; i++, j++)
        {
            playerStats[i] = weaponUpgrades[j];
        }

        // Set all levels to 1 (MAY NOT BE NEEDED!!!!!!!!!!!!!!!!!!!!!!!!!!)
        foreach (PlayerUpgrade stat in playerStats)
        {
            //print(stat.upgradeName);
            //stat.SetLevel(1);
        }

        // Disable all weapons (MAY NOT BE NEEDED!!!!!!!!!!!!!!!!!!!!!!!!!!)
        foreach (PlayerUpgrade weapon in weaponUpgrades)
        {
            weapon.isActive = false;
        }
    }

    void Start()
    {
        // Enable selected starting weapon
        // This needs to be in start, because PlayerUpgrades set isActive = false in OnEnable
        if (!startWithoutWeapons)
            weaponUpgrades[startWeaponIndex].IncreaseLevel(); // This will activate it

        isReady = true;
    }

    public bool IsReady() => isReady;

    public void StartUpgradeScreen()
    {
        UIManager.Instance.PauseGame(true);
        upgradeScreen.SetActive(true);
            
        // Build the list of available upgrades (ones that aren't max level)
        BuildPossibleUpgradeList();

        // Get array of selected options (if not enough, will set null elements)
        PlayerUpgrade[] randomizedUpgrades = GetRandomChoices();

        for (int i = 0; i < choiceOptions.Length; i++)
        {
            // Pass to choice buttons (including null values). If null, choice script will make it give health
            choiceOptions[i].SetInfo(randomizedUpgrades[i]);
        }
    }

    void BuildPossibleUpgradeList()
    {
        // Empty the list
        possibleUpgrades.Clear();

        foreach (PlayerUpgrade stat in playerStats)
        {
            // If the stat isn't max level, or if it isn't active, add it to the list
            if (stat.IsMaxLevel() == false || stat.isActive == false)
            {
                possibleUpgrades.Add(stat);
            }
            // If it is max level, print if desired
            else if (printDebug)
            {
                Debug.LogWarning(stat.upgradeName + " is level " + stat.GetLevel() + "! Make sure it's in the Upgrade Manager's list");
            }
        }
    }
    
    PlayerUpgrade[] GetRandomChoices()
    {
        PlayerUpgrade[] selectedUpgrades = new PlayerUpgrade[choiceOptions.Length];

        for (int i = 0; i < choiceOptions.Length; i++)
        {
            //If there aren't enough possible upgrades, set null
            if (possibleUpgrades.Count < 1)
            {
                selectedUpgrades[i] = null;
            }
            else
            {
                //Get random index in the list
                int rand = Random.Range(0, possibleUpgrades.Count);

                //Set to the new array
                selectedUpgrades[i] = possibleUpgrades[rand];

                //Remove from the list
                possibleUpgrades.RemoveAt(rand);
            }
        }

        return selectedUpgrades;
    }
}
