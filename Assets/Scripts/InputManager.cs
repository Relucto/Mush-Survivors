using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

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

    public InputActionAsset inputActions;
    public string mapName, moveName;
    InputActionMap playerMap;
    InputAction moveAction;

    bool isReady;

    void OnEnable()
    {
        playerMap = inputActions.FindActionMap(mapName);

        moveAction = playerMap.FindAction(moveName);

        playerMap.Enable();
    }

    void OnDisable()
    {
        playerMap.Disable();
    }

    void Start()
    {
        isReady = true;
    }

    public bool IsReady() => isReady;

    public Vector2 ReadMoveInput()
    {
        return moveAction.ReadValue<Vector2>();
    }
}
