using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUpgrade", menuName = "Scriptable Objects/PlayerUpgrade")]
public class PlayerUpgrade : ScriptableObject
{
    [Header("Display")]
    public string upgradeName = "";
    public Sprite displaySprite;
    public bool printDebug;

    [Header("Stats")]
    public UpgradeType type;
    public LevelStatGroup[] levelValueGroup;

    public event Action levelUp, requestActivation;
    
    [HideInInspector]
    public bool isActive;

    [System.Serializable]
    public class LevelStatGroup
    {
        public LevelStat[] stats;
    }

    [System.Serializable]
    public class LevelStat
    {
        public string statName;
        public float value;
    }

    public enum UpgradeType { PlayerStat, Weapon }

    int level = 1;

    void OnEnable()
    {
        level = 1;

        if (type == UpgradeType.Weapon)
        {
            isActive = false;
        }
        else
        {
            isActive = true;
        }

        if (printDebug)
        {
            string errorMessage = "";

        if (displaySprite == null)
            errorMessage += "Missing Sprite! ";
        if (upgradeName.Equals(""))
            errorMessage += "Missing Name! ";
        if (!errorMessage.Equals(""))
            Debug.LogWarning(name + ": " + errorMessage);

        Debug.Log(name + ": Level " + level);
        }
    }

    public bool IsMaxLevel()
    {
        return level >= levelValueGroup.Length ? true : false;
    }

    public void IncreaseLevel()
    {
        //If not already active
        if (isActive == false)
        {
            isActive = true;

            requestActivation?.Invoke();

            return;
        }
        
        if (level >= levelValueGroup.Length)
            return;

        level++;

        levelUp?.Invoke();
    }

    public LevelStatGroup GetLevelValue()
    {
        if (levelValueGroup.Length < 1)
        {
            Debug.LogError(name + ": array too small");
            Debug.Break();
            return null;
        }

        return levelValueGroup[level - 1];
    }

    public int GetLevel()
    {
        return level;
    }
    
    public void SetLevel(int newLevel)
    {
        level = newLevel;
    }
}
