using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioSystem;

public class GameManager : MonoBehaviour
{
    public Animator fader;
    public TMP_Text timer;
    public PlayerController playerController;
    public EnemySpawner spawner;
    public GameObject deathScreen;
    public WeaponAnimator vine;
    public SporeBulletLauncher sporeLauncher;
    public AudioChannel musicChannel;
    public AudioPair music;

    [Header("Ready Objects")]
    public bool showReadyStatus;

    public ReadyObject[] readyObjects;

    [System.Serializable]
    public struct ReadyObject
    {
        [HideInInspector]
        public string name;
        public GameObject obj;
        public MonoBehaviour[] scripts;
        public bool disabled;
    }

    float currentSeconds;
    int seconds, minutes;
    string displaySeconds, displayMinutes;
    bool timerActive;

    void OnValidate()
    {
        for (int i = 0; i < readyObjects.Length; i++)
        {
            if (readyObjects[i].obj == null)
            {
                readyObjects[i].name = readyObjects[i].scripts[0].gameObject.name;
            }
            else
            {
                readyObjects[i].name = readyObjects[i].obj.name;
            }
            
            foreach (MonoBehaviour mono in readyObjects[i].scripts)
            {
                mono.enabled = false;
            }
        }
    }

    void Awake()
    {
        //Disable all objects and scripts
        foreach (ReadyObject entry in readyObjects)
        {
            if (entry.obj != null)
                entry.obj.SetActive(false);

            foreach (MonoBehaviour mono in entry.scripts)
            {
                mono.enabled = false;
            }
        }
    }

    void Start()
    {
        currentSeconds = 0;
        timerActive = false;

        StartCoroutine(ReadyUp());

        musicChannel.Play(music.clip, music.volume);
    }

    IEnumerator ReadyUp()
    {
        // For each entry
        foreach (ReadyObject entry in readyObjects)
        {
            if (entry.disabled)
                continue;
                
            // Enable object
            if (entry.obj != null)
                entry.obj.SetActive(true);
            
            // Enable scripts
            foreach (MonoBehaviour mono in entry.scripts)
            {
                PrintReadyMessage("Starting " + mono.GetType().Name);

                // Make sure it has the IAwaitable component
                if (mono.TryGetComponent(out IAwaitable awaitable))
                {
                    mono.enabled = true;

                    float elapsedTime = 0;

                    // Wait for it to be ready
                    do
                    {
                        elapsedTime += Time.deltaTime;

                        if (elapsedTime > 5)
                        {
                            Debug.LogWarning($"{mono.GetType().Name} is taking longer than expected... ({awaitable.IsReady()})");
                        }

                        yield return null;
                    } while (awaitable.IsReady() == false);

                    PrintReadyMessage($"{mono.GetType().Name} is ready.");
                }
                else
                {
                    Debug.LogError($"{mono.GetType().Name}.cs does not contain IAwaitable on GameObject: {entry.obj.name}!");
                    Debug.Break();
                }
            }
        }

        PrintReadyMessage("Everything is ready!");

        UIManager.Instance.PauseGame(false); // Hides mouse cursor

        fader.Play("FadeIn", -1, 0);

        yield return new WaitForSeconds(1); // Wait for fade in

        StartEverything();
    }

    void StartEverything()
    {
        PlayerController.isActive = true; // Enable player control

        timerActive = true;

        spawner.StartSpawning();
    }
    
    void PrintReadyMessage(string message)
    {
        if (showReadyStatus)
            print(message);
    }

    void Update()
    {
        if (timerActive)
        {
            UpdateTimer();
        }
    }

    void UpdateTimer()
    {
        currentSeconds += Time.deltaTime;

        seconds = (int)currentSeconds;
        
        if (currentSeconds >= 60)
        {
            currentSeconds = 0;
            minutes++;
        }

        displayMinutes = minutes < 10 ? "0" + minutes.ToString() : minutes.ToString();
        displaySeconds = currentSeconds < 10 ? "0" + seconds.ToString() : seconds.ToString();

        timer.text = displayMinutes + ":" + displaySeconds;
    }

    public void OnPlayerDeath()
    {
        spawner.StopSpawning();
        playerController.enabled = false;
        timerActive = false;

        // Stop all weapons
        vine.DisableSelf();
        sporeLauncher.DisableSelf();

        // Enable death screen
        deathScreen.SetActive(true);
        
        StartCoroutine(FadeOut());

        IEnumerator FadeOut()
        {
            yield return new WaitForSeconds(3);

            UIManager.Instance.ChangeScene("MainMenu");

            /*
            fader.Play("FadeOut");

            yield return new WaitForSeconds(1.5f);

            SceneManager.LoadScene("MainMenu");*/
        }
    }
}
