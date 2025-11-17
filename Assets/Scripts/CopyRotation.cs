using UnityEngine;

public class CopyRotation : MonoBehaviour
{
    public Transform origin;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = origin.rotation;
    }
}
