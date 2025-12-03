using UnityEngine;

public class InteractZone : MonoBehaviour
{
    public GameObject activateObj;
    public GameObject eToInteractText;
    public GameObject frontLight;

    bool active;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            print("enter");
            active = true;
            eToInteractText.SetActive(true);
            frontLight.SetActive(false);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            print("Exit");
            active = false;
            eToInteractText.SetActive(false);
            frontLight.SetActive(true);
        }
    }

    void Update()
    {
        if (active)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (activateObj.activeInHierarchy == false)
                {
                    activateObj.SetActive(true);
                    PlayerController.isActive = false;
                    // disable camera movement
                    eToInteractText.SetActive(false);
                    frontLight.SetActive(false);
                }
            }
        }
    }
}
