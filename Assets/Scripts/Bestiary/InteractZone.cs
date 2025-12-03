using UnityEngine;

public class InteractZone : MonoBehaviour
{
    public GameObject[] activateObj, deactivateObj;

    bool active;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            print("enter");
            active = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            print("Exit");
            active = false;
        }
    }
}
