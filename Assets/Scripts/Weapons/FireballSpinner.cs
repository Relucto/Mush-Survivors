using System.Collections.Generic;
using UnityEngine;
using ElementalEffects;

public class FireballSpinner : MonoBehaviour
{
    public PlayerUpgrade fireballStats;
    public List<FireballCollection> fireballCollections;
    
    [System.Serializable]
    public class FireballCollection
    {
        public List<GameObject> fireballs;
    }
    
    public class HitObject
    {
        public Collider collider;
        public float cooldown;
    }

    Animator anim;
    List<HitObject> hitObjects = new List<HitObject>();
    float damage;
    float speed;
    int fireballActiveIndex;
    bool isActive;

    void OnEnable()
    {
        fireballStats.levelUp += SetStats;
        fireballStats.requestActivation += ActivateSelf;
    }

    void OnDisable()
    {
        fireballStats.levelUp -= SetStats;
        fireballStats.requestActivation -= ActivateSelf;
    }

    void Start()
    {
        // Disable all fireballs to start
        foreach (FireballCollection collection in fireballCollections)
        {
            foreach (GameObject fireball in collection.fireballs)
            {
                fireball.SetActive(false);
            }
        }

        anim = GetComponent<Animator>();

        isActive = false;
        fireballStats.SetLevel(1);

        SetStats();
    }

    void Update()
    {
        for (int i = hitObjects.Count - 1; i >= 0; i--)
        {
            hitObjects[i].cooldown -= Time.deltaTime;
            if (hitObjects[i].cooldown <= 0)
            {
                hitObjects.RemoveAt(i);
            }
        }
    }

    void ActivateSelf()
    {
        isActive = true;

        EnableFireballs(0); // Start index (level 1)
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!isActive)
            return;

        if (collider.CompareTag("Enemy") || collider.CompareTag("Plant"))
        {
            foreach (HitObject obj in hitObjects)
            {
                if (obj.collider == collider)
                    return;
            }

            float critDamage = WeaponHandler.CritDamage();
            bool isCritical = critDamage == 0 ? false : true;

            IDamageable damageable = collider.GetComponent<IDamageable>();

            damageable.Damage(damage + critDamage, isCritical, DamageType.fire, true);

            HitObject enemy = new HitObject();
            enemy.collider = collider;
            enemy.cooldown = 1;
            hitObjects.Add(enemy);
        }
    }

    void SetStats()
    {
        PlayerUpgrade.LevelStatGroup statGroup = fireballStats.GetLevelValue();

        damage = statGroup.stats[0].value; // Damage
        speed = statGroup.stats[1].value; // Speed
        fireballActiveIndex = (int)statGroup.stats[2].value; // Index in fireball list

        anim.speed = speed;

        if (isActive)
        {
            // Enable fireballs
            EnableFireballs(fireballActiveIndex);
        }
    }

    void EnableFireballs(int index)
    {
        for (int i = 0; i <= index; i++)
        {
            foreach (GameObject fireball in fireballCollections[i].fireballs)
            {
                fireball.SetActive(true);
            }
        }
    }
}
