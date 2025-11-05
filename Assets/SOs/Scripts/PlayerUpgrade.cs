using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUpgrade", menuName = "Scriptable Objects/PlayerUpgrade")]
public class PlayerUpgrade : ScriptableObject
{
    [Header("Display")]
    public string upgradeName = "Upgrade";
    public Sprite displaySprite;
    //public float[] levelValues;
    public LevelStatGroup[] levelValueGroup;
    public event Action<LevelStatGroup> levelUp;

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
        if (level >= levelValueGroup.Length)
            return;

        level++;

        levelUp?.Invoke(GetLevelValue());
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
