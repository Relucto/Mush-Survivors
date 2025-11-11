using UnityEngine;

public class XPOrb : MonoBehaviour
{
    public float lifeTime;
    public float value;

    public void SetXPValue(float num)
    {
        float randomness = num * 0.2f;
        value = Random.Range(num - randomness, num + randomness);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lifeTime -= Time.fixedDeltaTime;

        if (lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }
}
