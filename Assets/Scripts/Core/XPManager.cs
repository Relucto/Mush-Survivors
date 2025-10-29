using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance { get; private set; }

    public int baselineXPRequirement;
    public float subsequentLevelMult = 1;
    public UnityEvent<int> playerLevelUp;
    public ProgressBar xpBar;
    public TMP_Text levelText, xpText;

    float playerXP;
    float requiredXP;
    int playerLevel;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        requiredXP = baselineXPRequirement;

        //Set UI elements
        xpBar.SetMaxValue(requiredXP);
        xpBar.SetValue(-1);

        UpdateLevelText();
        UpdateXPText();
    }

    public void AddXP(float value)
    {
        playerXP += value;

        if (playerXP >= requiredXP)
        {
            //Reset player xp
            playerXP = 0;

            //Increase level requirement
            requiredXP *= subsequentLevelMult;
            xpBar.SetMaxValue(requiredXP);

            //Increase level
            playerLevel++;

            //Change Level text
            UpdateLevelText();
            
            //Level up (event?)
            playerLevelUp?.Invoke(playerLevel);
        }

        UpdateXPText();
        xpBar.SetValue(playerXP);
    }

    void UpdateLevelText()
    {
        levelText.text = "Level: " + playerLevel;
    }

    void UpdateXPText()
    {
        xpText.text = playerXP.ToString("F1") + " / " + requiredXP.ToString("F1");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            AddXP(10);
        }
    }
}
