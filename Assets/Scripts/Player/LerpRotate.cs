using UnityEngine;

public class LerpRotate : MonoBehaviour
{
    public float lerpRate;
    Vector3 targetRotation;

    public void SetRotation(Vector2 rotation)
    {
        targetRotation = new Vector3(rotation.x, 0, rotation.y);
    }

    void Update()
    {
        if (transform.rotation.eulerAngles != targetRotation)
        {
            Vector3 newRotation = Vector3.Lerp(transform.rotation.eulerAngles, targetRotation, lerpRate * Time.deltaTime);

            transform.rotation = Quaternion.Euler(newRotation);
        }
    }
}
