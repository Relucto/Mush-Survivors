using TMPro;
using UnityEngine;
using UnityEngine.Events;
using AudioSystem;

public class XPManager : MonoBehaviour, IAwaitable
{
    public static XPManager Instance { get; private set; }

    public int baselineXPRequirement;
    public float subsequentLevelMult = 1;
    public UnityEvent<int> onPlayerLevelUp;
    public ProgressBar xpBar;
    public TMP_Text levelText; //xpText;
    public bool XForXP;

    float playerXP;
    float requiredXP;
    int playerLevel;
    bool isReady = false, isActive;

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
        //UpdateXPText();

        isActive = true;

        isReady = true;
    }

    void Update()
    {
        if (XForXP)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                AddXP(10);
            }
        }   
    }

    public bool IsReady() => isReady;

    public void AddXP(float value)
    {
        if (!isActive)
            return;
            
        playerXP += value;

        if (playerXP >= requiredXP)
        {
            playerXP = requiredXP;

            //Reset player xp
            //playerXP = 0;

            //Increase level
            playerLevel++;

            //Change Level text
            UpdateLevelText();

            // Call level up event
            onPlayerLevelUp?.Invoke(playerLevel);

            GetComponent<AudioSource>().Play();
        }

        UpdateXPText();
        xpBar.SetValue(playerXP);
    }
    
    public void ResetToZero()
    {
        playerXP = 0;

        //Increase level requirement
        requiredXP *= subsequentLevelMult;
        xpBar.SetMaxValue(requiredXP);

        AddXP(0); // Go through and reset bar and text
    }

    void UpdateLevelText()
    {
        levelText.text = "Level: " + playerLevel;
    }

    void UpdateXPText()
    {
        //xpText.text = playerXP.ToString("F2") + " / " + requiredXP.ToString("F2");
    }

    public void Stop() => isActive = false;
}
