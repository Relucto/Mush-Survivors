using UnityEngine;

public class SporeBullet : MonoBehaviour
{
    public float moveSpeed;
    
    [HideInInspector] public Vector3 direction;

    void Update()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
}
