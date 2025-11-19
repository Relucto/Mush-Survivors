using UnityEngine;

public class XPConsumer : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("XP"))
        {
            collider.GetComponent<XPOrb>().Consume();
        }
    }
}
