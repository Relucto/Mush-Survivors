using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnLocation;

    bool spawning;

    void Start()
    {

    }

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

        IEnumerator Spawning()
        {
            Debug.LogWarning("I'm not set up yet");
            yield return null;
        }
    }
}
