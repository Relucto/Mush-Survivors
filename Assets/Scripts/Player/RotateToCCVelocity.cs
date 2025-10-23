using UnityEngine;

public class RotateToCCVelocity : MonoBehaviour
{
    public float rotateSpeed;
    Vector3 targetRotation;
    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 horizontalMovement = new Vector3(controller.velocity.x, 0, controller.velocity.z);

        if (horizontalMovement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(horizontalMovement, Vector3.up);

            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
        }
    }
}
