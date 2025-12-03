using UnityEngine;

public class EntryExiter : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            gameObject.SetActive(false);
            PlayerController.isActive = true;
            //enable camera movement
        }
    }
}
