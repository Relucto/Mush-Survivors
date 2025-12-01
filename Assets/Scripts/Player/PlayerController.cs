using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IAwaitable, IEntity
{
    [Header("Controls")]
    public InputActionAsset inputActions;
    float moveSpeed;
    public float jumpHeight;
    public float gravityMultiplier = 1;

    [Header("Animations")]
    public Animator playerModelAnimator;
    public string walkBool, jumpTrigger;

    [Header("Stats")]
    public PlayerUpgrade speedStats;

    [HideInInspector]
    public static bool isActive = false;

    Vector3 playerVelocity, wallPush;
    Transform mainCamera;
    CharacterController controller;
    const float gravityValue = -9.81f;
    float characterRadius;

    bool isReady = false, isDead, grounded, onWall;

    void OnEnable()
    {
        speedStats.levelUp += LevelUpSpeed;
    }

    void OnDisable()
    {
        speedStats.levelUp -= LevelUpSpeed;
    }

    void Start()
    {
        mainCamera = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        characterRadius = controller.radius * transform.localScale.x;

        speedStats.SetLevel(1);
        SetSpeed(speedStats.GetLevelValue().stats[0].value);

        isReady = true;
    }

    public bool IsReady()
    {
        return isReady;
    } 

    void Update()
    {
        if (isActive == false)
            return;

        grounded = IsGrounded();
            
        if (grounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -5f;
        }

        

        //====================
        //Horizontal Movement

        Vector2 moveInput = InputManager.Instance.ReadMoveInput();
        Vector2 adjustedInputVector = GetDirectionVector(moveInput);
        Vector3 move = !onWall ? new Vector3(adjustedInputVector.x, 0, adjustedInputVector.y) : Vector3.zero;

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

        if (InputManager.Instance.ReadJump() && grounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue * gravityMultiplier);
            playerModelAnimator.SetTrigger(jumpTrigger);
        }

        //====================
        //Apply gravity

        playerVelocity.y += gravityValue * gravityMultiplier * Time.deltaTime;

        //====================
        //Combine and apply movement

        Vector3 moveVector = (move * moveSpeed) + (wallPush * 3) + (playerVelocity.y * Vector3.up);
        
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

    bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, characterRadius, Vector3.down, out hit, 1.2f))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);

            if (angle < controller.slopeLimit)
            {
                onWall = false;
                wallPush = Vector3.zero;
                return true;
            }
            else
            {
                onWall = true;
                wallPush = new Vector3(hit.normal.x, 0, hit.normal.z).normalized;
            }
        }
        else
        {
            onWall = false;
        }

        return false;
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

    void SetSpeed(float value) => moveSpeed = value;

    void LevelUpSpeed()
    {
        PlayerUpgrade.LevelStatGroup statGroup = speedStats.GetLevelValue();

        if (statGroup.stats.Length != 1)
        {
            Debug.LogError(speedStats.name + " has incorrect number of values");
            Debug.Break();
            return;
        }

        SetSpeed(statGroup.stats[0].value);
    }

    public void SlowSpeed(float value)
    {
        
    }

    public void NormalSpeed()
    {
        
    }
}
