using UnityEngine;

public class DamageIndicatorManager : MonoBehaviour
{
    [Header("Gradient")]
    public Gradient damageGradient;
    public float damageLow;
    public float damageHigh;

    [Header("Pool")]
    public GameObject prefab;
    public Transform poolParent;
    public int startSize, maxSize;

    public static DamageIndicatorManager Instance { get; private set; }

    void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    Pool pool;

    void Start()
    {
        pool = new Pool(prefab, startSize, maxSize, poolParent);
    }

    public void SpawnIndicator(Vector3 spawnPoint, float value, bool isCritical)
    {
        GameObject obj = pool.Get();
        obj.transform.position = spawnPoint;

        Color color = isCritical ? Color.red : damageGradient.Evaluate(Mathf.InverseLerp(damageLow, damageHigh, value));

        DamageIndicator indicator = obj.GetComponent<DamageIndicator>();
        indicator.SetValue(value, color);

        if (isCritical)
        {
            indicator.GetComponent<Animator>().speed = 0.5f;
            indicator.transform.localScale = Vector3.one * 2;
        }
    }

    public void ReturnIndicator(GameObject obj)
    {
        pool.Return(obj);
    }
}
