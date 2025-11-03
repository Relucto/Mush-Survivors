using UnityEngine;

public class UIManager : MonoBehaviour, IAwaitable
{
    public static UIManager Instance { get; private set; }

    bool isReady;
    public bool IsReady() => isReady;

    void Awake()
    {
        if (Instance == null)
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
    }

    void Start()
    {
        isReady = true;
    }

    public void PauseGame(bool value)
    {
        Time.timeScale = value == true ? 0 : 1;

        Cursor.visible = value;

        if (value == true)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }
}
