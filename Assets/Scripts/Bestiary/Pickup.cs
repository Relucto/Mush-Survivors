using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    public UnityEvent onPickup;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            onPickup?.Invoke();
        }
    }
}
