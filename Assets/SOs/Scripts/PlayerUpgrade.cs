using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUpgrade", menuName = "Scriptable Objects/PlayerUpgrade")]
public class PlayerUpgrade : ScriptableObject
{
    [Header("Display")]
    public string upgradeName = "Upgrade";
    public Sprite displaySprite;
    public float[] levelValues;
    public event Action<float> levelUp;

    int level = 1;

    public bool IsMaxLevel()
    {
        return level >= levelValues.Length ? true : false;
    }

    public void IncreaseLevel()
    {
        if (level >= levelValues.Length)
            return;

        level++;

        levelUp?.Invoke(GetLevelValue());
    }

    public float GetLevelValue()
    {
        if (levelValues.Length < 1)
        {
            Debug.LogError(name + ": array too small");
            Debug.Break();
            return -999;
        }

        return levelValues[level - 1];
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
