using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;

    void Update()
    {
        transform.position = target.position + offset;
    }
}
