using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionAsset inputActions;
    public float moveSpeed;
    public Animator playerModelAnimator;

    Transform mainCamera;
    CharacterController controller;

    bool isReady;

    void Start()
    {
        mainCamera = Camera.main.transform;
        controller = GetComponent<CharacterController>();

        isReady = true;
    }

    public bool IsReady() => isReady;

    void Update()
    {
        Vector2 moveInput = InputManager.Instance.ReadMoveInput();

        if (moveInput.Equals(Vector2.zero))
        {
            playerModelAnimator.SetBool("Moving", false);
        }
        else
        {
            playerModelAnimator.SetBool("Moving", true);
        }

        Vector2 moveDirection = GetDirectionVector(moveInput);

        controller.Move(new Vector3(moveDirection.x, 0, moveDirection.y) * moveSpeed * Time.deltaTime);

        //Rotate in moving direction
        
    }

    Vector2 GetDirectionVector(Vector2 inputVector)
    {
        //Add starting angle with the camera angle to get new forward direction
        float startAngle = Mathf.Atan(inputVector.y / inputVector.x); // Inverse tangent to get ANGLE (in radians)

        //Tangent only works for 1st and 4th quadrant. Add PI to get it on the -x side
        if (inputVector.x < 0)
            startAngle += Mathf.PI;

        //Add angles together (radians)
        float finalAngle = startAngle - (mainCamera.rotation.eulerAngles.y * Mathf.Deg2Rad);

        //Convert angle to vector2
        return new Vector2(Mathf.Cos(finalAngle), Mathf.Sin(finalAngle)).normalized;
    }
}
