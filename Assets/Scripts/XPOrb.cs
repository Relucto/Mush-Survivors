using UnityEngine;

public class XPOrb : MonoBehaviour
{
    public float lifeTime;
    public float value;
    public float lerpSpeed;

    Transform playerT;

    bool moving;

    public void Start()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        moving = false;
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
        XPManager.Instance.AddXP(value);
        Destroy(gameObject);
    }

    public void SetXPValue(float num)
    {
        float randomness = num * 0.2f;
        value = Random.Range(num - randomness, num + randomness);
    }
}
