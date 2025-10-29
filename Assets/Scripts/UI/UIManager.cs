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

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }
}
