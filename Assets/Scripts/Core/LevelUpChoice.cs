using AudioSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpChoice : MonoBehaviour
{
    [SerializeField] AudioChannel sfx;
    [SerializeField] AudioPair selectSound;
    [SerializeField] TMP_Text nameText, levelText;
    [SerializeField] Image displayImage;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] string defaultName, defaultDescription, introEffect, lvlEffect;
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

            nameText.text = introEffect + defaultName;
            levelText.text = introEffect + defaultDescription;
            displayImage.overrideSprite = defaultSprite;

            return;
        }

        //Set working variables
        statToUpgrade = upgrade;
        int level = statToUpgrade.GetLevel();

        //Set display variables
        if (statToUpgrade.isActive)
        {
            nameText.text = introEffect + statToUpgrade.upgradeName;
            levelText.text = introEffect + "Level " + lvlEffect + level + " -> " + (level + 1);
            displayImage.overrideSprite = statToUpgrade.displaySprite;
        }
        //If this hasn't been unlocked yet
        else
        {
            nameText.text = introEffect + statToUpgrade.upgradeName;
            levelText.text = introEffect + "<wave><palette>NEW!";
            displayImage.overrideSprite = statToUpgrade.displaySprite;
        }
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

        XPManager.Instance.ResetToZero();
        
        UIManager.Instance.PauseGame(false);

        sfx.Play(selectSound.clip, selectSound.volume, selectSound.pitchVariance, GameObject.FindGameObjectWithTag("Player").transform.position);
    }
}
