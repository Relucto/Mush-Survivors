using System.Collections.Generic;
using UnityEngine;

public class XPPuller : MonoBehaviour
{
    public PlayerUpgrade pickupStats;
    public SphereCollider pickupCollider;
    public float suckSpeed;

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
            if (!pulledObjects.Contains(collider.transform))
            {
                pulledObjects.Add(collider.transform);
            }
        }
    }
    
    void Update()
    {
        float step = suckSpeed * Time.deltaTime;

        foreach (Transform xp in pulledObjects)
        {
            xp.position = Vector3.MoveTowards(xp.position, transform.position, step);
        }
    }
}
