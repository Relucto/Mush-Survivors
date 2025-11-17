using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Animator fader;

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
    }

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
        StartCoroutine(ReadyUp());
    }

    IEnumerator ReadyUp()
    {
        // For each entry
        foreach (ReadyObject entry in readyObjects)
        {
            // Enable object
            if (entry.obj != null)
                entry.obj.SetActive(true);
            
            // Enable scripts
            foreach (MonoBehaviour mono in entry.scripts)
            {
                PrintMessage("Starting " + mono.GetType().Name);

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

                    PrintMessage($"{mono.GetType().Name} is ready.");
                }
                else
                {
                    Debug.LogError($"{mono.GetType().Name}.cs does not contain IAwaitable on GameObject: {entry.obj.name}!");
                    Debug.Break();
                }
            }
        }

        PrintMessage("Everything is ready!");

        UIManager.Instance.PauseGame(false); // Hides mouse cursor

        fader.Play("FadeIn", -1, 0);

        yield return new WaitForSeconds(1); // Wait for fade in
        PlayerController.isActive = true; // Enable player control
    }
    
    void PrintMessage(string message)
    {
        if (showReadyStatus)
            print(message);
    }
}
