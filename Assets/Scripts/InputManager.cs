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
    public string mapName, moveName, jumpName;
    InputActionMap playerMap;
    InputAction moveAction, jumpAction;

    bool isReady;

    void OnEnable()
    {
        playerMap = inputActions.FindActionMap(mapName);

        moveAction = playerMap.FindAction(moveName);

        jumpAction = playerMap.FindAction(jumpName);

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

    public bool ReadJump()
    {
        return jumpAction.WasPressedThisFrame();
    }
}
