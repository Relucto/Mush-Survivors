using UnityEngine;

public class DamageIndicatorManager : MonoBehaviour
{
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

    public GameObject SpawnIndicator(Vector3 spawnPoint)
    {
        GameObject indicator = pool.Get();
        indicator.transform.position = spawnPoint;
        return indicator;
    }

    public void ReturnIndicator(GameObject obj)
    {
        pool.Return(obj);
    }
}
