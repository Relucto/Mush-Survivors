using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUpgrade", menuName = "Scriptable Objects/PlayerUpgrade")]
public class PlayerUpgrade : ScriptableObject
{
    [Header("Display")]
    public string upgradeName = "Upgrade";
    public Sprite displaySprite;
    public bool isActive;
    public LevelStatGroup[] levelValueGroup;
    public event Action levelUp, requestActivation;

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

    int level = 1;

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
