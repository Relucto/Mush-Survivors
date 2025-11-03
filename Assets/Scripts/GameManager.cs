using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Cache")]
    public bool showReadyStatus;
    public InputManager inputManager;
    public UpgradeManager upgradeManager;
    public PlayerController playerController;
    public Health health;
    public UIManager uiManager;

    List<MonoBehaviour> readyList = new List<MonoBehaviour>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIManager.Instance.PauseGame(false);

        //BuildReadyList();

        //StartCoroutine(ReadyUp());
    }
    
    void BuildReadyList()
    {
        readyList.Add(inputManager);
        readyList.Add(upgradeManager);
        readyList.Add(playerController);
        readyList.Add(health);
        readyList.Add(uiManager);
    }

    IEnumerator ReadyUp()
    {
        foreach (MonoBehaviour script in readyList)
        {
            float elapsedTime = 0;

            PrintMessage("Starting " + script.GetType().Name);
            script.enabled = true;

            IAwaitable awaitable = script.GetComponent<IAwaitable>();

            while (!awaitable.IsReady())
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime > 5)
                {
                    Debug.LogWarning(script.GetType().Name + " is taking a while to start...");
                    break;
                }

                yield return null;
            }

            PrintMessage(script.GetType().Name + " is ready");
        }

        PrintMessage("Everything is ready");
    }
    
    void PrintMessage(string message)
    {
        if (showReadyStatus)
            print(message);
    }
}
