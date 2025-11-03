using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour, IAwaitable
{
    public PlayerUpgrade[] playerStats;

    public GameObject upgradeScreen;
    public LevelUpChoice[] choiceOptions;
    public bool printDebug;

    List<PlayerUpgrade> possibleUpgrades = new List<PlayerUpgrade>();

    bool isReady;

    void Awake()
    {
        foreach (PlayerUpgrade stat in playerStats)
        {
            stat.SetLevel(1);
        }
    }

    void Start()
    {
        isReady = true;
    }

    public bool IsReady() => isReady;

    // Temporary as FUCK
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            //StartUpgradeScreen();
        }
    }

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
            // If the stat isn't max level, add it to the list
            if (stat.IsMaxLevel() == false)
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
