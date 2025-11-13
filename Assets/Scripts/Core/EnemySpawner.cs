using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IAwaitable
{
    public GameObject enemyPrefab;
    public Transform spawnLocation;

    bool spawning, isReady;

    void Start()
    {
        isReady = true;
    }

    public bool IsReady() => isReady;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnLocation.position, Quaternion.identity);
    }

    public void StartSpawning()
    {
        spawning = true;

        StartCoroutine(Spawning());
    }
    
    IEnumerator Spawning()
    {
        Debug.LogWarning("I'm not set up yet (" + name + ")");
        yield return null;
    }
}
