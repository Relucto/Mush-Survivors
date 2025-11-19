using System.Collections.Generic;
using UnityEngine;

public class XPPuller : MonoBehaviour
{
    public PlayerUpgrade pickupStats;
    public SphereCollider pickupCollider;

    float radius;

    [HideInInspector]
    public List<Transform> pulledObjects = new List<Transform>();

    void OnValidate()
    {
        SetPickupDistance();
    }

    void Start()
    {
        SetPickupDistance();
    }

    public void SetPickupDistance()
    {
        radius = pickupStats.GetLevelValue().stats[0].value;

        pickupCollider.radius = radius;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("XP"))
        {
            collider.GetComponent<XPOrb>().EnableMove();
        }
    }
}
