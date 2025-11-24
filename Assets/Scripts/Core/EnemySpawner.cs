using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour, IAwaitable
{
    public int maxEnemies = 50;
    [HideInInspector] public static int numEnemies = 0;
    public EnemyType[] enemies;
    //public GameObject enemyPrefab;
    public Transform spawnLocation, player;

    NavMeshTriangulation triangulation;

    int totalChance;
    bool spawning, isReady;

    [System.Serializable]
    public struct EnemyType
    {
        public GameObject prefab;
        public int spawnChance;
    }

    void Start()
    {
        numEnemies = 0;
        totalChance = 0;
        
        foreach (EnemyType enemy in enemies)
        {
            totalChance += enemy.spawnChance;
        }

        triangulation = NavMesh.CalculateTriangulation();
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
        if (numEnemies >= maxEnemies)
            return;

        // Replace with pool?
        GameObject prefab = GetRandomEnemy();

        if (prefab == null)
            return;

        EnemyController enemy = Instantiate(prefab, spawnLocation.position, Quaternion.identity).GetComponent<EnemyController>();

        Vector3 spawnPosition = GetRandomPosition();

        // Error check
        if (spawnPosition == Vector3.zero)
        {
            Debug.LogWarning("Couldn't find a good spawn point...");

            // Replace with pool?
            Destroy(enemy.gameObject);
            return;
        }
        
        // Set position and enable
        enemy.agent.Warp(spawnPosition);
        enemy.playerT = player;
        enemy.agent.enabled = true;

        numEnemies++;
    }

    GameObject GetRandomEnemy()
    {
        int randomValue = Random.Range(0, totalChance);

        foreach (EnemyType enemy in enemies)
        {
            if (randomValue < enemy.spawnChance)
            {
                return enemy.prefab;
            }

            randomValue -= enemy.spawnChance;
        }

        Debug.LogError("SOMETHING WENT WRONG WITH THE RANDOM SPAWN CHANCE THING HELP");
        Debug.LogError("randomValue chosen was " + randomValue + "??");
        Debug.Break();
        return null;
    }


    Vector3 GetRandomPosition()
    {
        int vertIndex = Random.Range(0, triangulation.vertices.Length);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(triangulation.vertices[vertIndex], out hit, Mathf.Infinity, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return Vector3.zero;
        }
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
