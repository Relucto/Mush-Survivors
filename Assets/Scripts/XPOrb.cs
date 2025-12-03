using AudioSystem;
using UnityEngine;
using UnityEngine.UI;

public class XPOrb : MonoBehaviour
{
    public AudioChannel sfx;
    public AudioPair xpSound, healthSound;

    public float lifeTime;
    public float value;
    public float lerpSpeed;
    public GameObject potionParent, xpParent;
    public Image xpImage;
    public Sprite[] xpSprites;

    [HideInInspector] public PickupType myType;

    public enum PickupType {xp, health}

    Transform playerT;

    bool moving;

    public void Start()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        moving = false;

        xpImage.overrideSprite = xpSprites[Random.Range(0, xpSprites.Length)];
    }

    public void EnableMove()
    {
        moving = true;
    }

    public void Update()
    {
        if (moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerT.position, Time.deltaTime * 10);
        }
        else
        {
            lifeTime -= Time.deltaTime;

            if (lifeTime < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Consume()
    {
        if (myType == PickupType.xp)
        {
            XPManager.Instance.AddXP(value);
            sfx.Play(xpSound.clip, xpSound.volume, xpSound.pitchVariance, playerT.position);
        }
        else if (myType == PickupType.health)
        {
            // Heal player
            Health playerHealth = playerT.GetComponent<Health>();
            playerHealth.Heal(playerHealth.maxHealth / 12);

            DamageIndicatorManager.Instance.SpawnPlayerHeal(playerHealth.maxHealth / 12);
            //Play sound effect
            sfx.Play(healthSound.clip, healthSound.volume, healthSound.pitchVariance, playerT.position);
        }
        
        Destroy(gameObject);
    }

    public void ConvertToHealth()
    {
        xpParent.SetActive(false);
        potionParent.SetActive(true);
    }

    public void SetXPValue(float num)
    {
        float randomness = num * 0.2f;
        value = UnityEngine.Random.Range(num - randomness, num + randomness);
    }
}
