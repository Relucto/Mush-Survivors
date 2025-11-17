using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform cameraTransform;

    void OnEnable()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        transform.LookAt(transform.position + cameraTransform.forward);
    }
}
