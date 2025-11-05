using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpChoice : MonoBehaviour
{
    [SerializeField] TMP_Text displayText;
    [SerializeField] Image displayImage;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] string defaultText, introEffect, lvlEffect;
    [SerializeField] int healthIncreaseIfEmpty;
    [SerializeField] Health playerHealth;
    PlayerUpgrade statToUpgrade;

    // Called by the LevelUpScreenManager
    public void SetInfo(PlayerUpgrade upgrade)
    {
        //If null, make it the default sprite 
        if (upgrade == null)
        {
            statToUpgrade = null;

            print("There's an empty one, have some health instead :)");

            displayText.text = introEffect + defaultText;
            displayImage.overrideSprite = defaultSprite;

            return;
        }

        //Set working variables
        statToUpgrade = upgrade;
        int level = statToUpgrade.GetLevel();

        //Set display variables
        displayText.text = introEffect + statToUpgrade.upgradeName + ": Lvl " + lvlEffect + level + " -> " + (level + 1);
        displayImage.overrideSprite = statToUpgrade.displaySprite;
    }

    // Called by button script for each choice
    public void LevelUpSelected()
    {
        if (statToUpgrade != null)
        {
            statToUpgrade.IncreaseLevel();

            print("Leveling up " + statToUpgrade.name + " to level " + statToUpgrade.GetLevel());
        }
        else
        {
            //Give health instead
            playerHealth.Heal(healthIncreaseIfEmpty);
        }
        
        UIManager.Instance.PauseGame(false);
    }
}
