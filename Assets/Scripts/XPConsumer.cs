using UnityEngine;

public class XPConsumer : MonoBehaviour
{
    float xpGain;
    public XPPuller puller;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("XP"))
        {
            puller.pulledObjects.Remove(collider.transform);

            xpGain = collider.GetComponent<XPOrb>().value;

            Destroy(collider.gameObject);

            XPManager.Instance.AddXP(xpGain);
        }
    }
}
