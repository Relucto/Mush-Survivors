using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IAwaitable, IDamageable
{
    [Header("Controls")]
    public InputActionAsset inputActions;
    public PlayerUpgrade speedStats;
    float moveSpeed;
    public float jumpHeight;
    public float gravityMultiplier = 1;

    [Header("Animations")]
    public Animator playerModelAnimator;
    public string walkBool, jumpTrigger;

    Vector3 playerVelocity;
    Transform mainCamera;
    CharacterController controller;
    const float gravityValue = -9.81f;

    bool isReady = false, isDead;

    void OnEnable()
    {
        speedStats.levelUp += SetSpeed;
    }

    void OnDisable()
    {
        speedStats.levelUp -= SetSpeed;
    }

    void Start()
    {
        mainCamera = Camera.main.transform;
        controller = GetComponent<CharacterController>();

        SetSpeed(speedStats.GetLevelValue());

        isReady = true;
    }

    public bool IsReady()
    {
        return isReady;
    } 

    void Update()
    {
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        //====================
        //Horizontal Movement

        Vector2 moveInput = InputManager.Instance.ReadMoveInput();
        Vector2 adjustedInputVector = GetDirectionVector(moveInput);
        Vector3 move = new Vector3(adjustedInputVector.x, 0, adjustedInputVector.y);

        //====================
        //Set Animator state

        if (moveInput.Equals(Vector2.zero))
        {
            playerModelAnimator.SetBool(walkBool, false);
        }
        else
        {
            playerModelAnimator.SetBool(walkBool, true);
        }

        //====================
        //Jump

        if (InputManager.Instance.ReadJump() && controller.isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue * gravityMultiplier);
            playerModelAnimator.SetTrigger(jumpTrigger);
        }

        //====================
        //Apply gravity

        playerVelocity.y += gravityValue * gravityMultiplier * Time.deltaTime;

        //====================
        //Combine and apply movement

        Vector3 moveVector = (move * moveSpeed) + (playerVelocity.y * Vector3.up);
        
        controller.Move(moveVector * Time.deltaTime);
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

    public void ReactToDamage()
    {
        if (isDead)
            return;

        //React however needed
    }

    public void Die()
    {
        if (isDead == false)
        {
            isDead = true;
            print("Player died");
        }
    }

    //========================================
    //Upgrade functions

    void SetSpeed(float value)
    {
        moveSpeed = value;
    }
}
