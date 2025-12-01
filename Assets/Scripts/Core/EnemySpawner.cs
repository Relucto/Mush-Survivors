using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour, IAwaitable
{
    public static EnemySpawner Instance { get; private set; }

    public int maxEnemies = 50;
    public float waitTime = 0.5f;
    public float plantRespawnCooldown;
    [HideInInspector] public static int numEnemies = 0;
    public EnemyType[] enemies;
    //public GameObject enemyPrefab;
    public Transform spawnLocation, player;
    public GameObject plantSpawnsParent;

    List<Vector3> plantSpawns = new List<Vector3>();
    List<Vector3> takenSpawns = new List<Vector3>();
    List<RemoveEntry> removeEntries = new List<RemoveEntry>();

    public class RemoveEntry
    {
        public Vector3 position;
        public float timer;
    }

    NavMeshTriangulation triangulation;

    int totalChance;
    bool spawning, isReady;

    [System.Serializable]
    public struct EnemyType
    {
        public GameObject prefab;
        public int spawnChance;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
                Destroy(gameObject);
        }
    }

    void Start()
    {
        numEnemies = 0;
        totalChance = 0;

        BuildPlantSpawns();
        
        foreach (EnemyType enemy in enemies)
        {
            totalChance += enemy.spawnChance;
        }

        triangulation = NavMesh.CalculateTriangulation();

        /*
        foreach (EnemyType enemy in enemies)
        {
            GameObject obj = Instantiate(enemy.prefab, new Vector3(-1000, 0, 0), Quaternion.identity);
        } */

        isReady = true;
    }

    public bool IsReady() => isReady;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnEnemy();
        }

        // Don't let plant spawn in same spot after one just died
        for (int i = removeEntries.Count - 1; i >= 0; i--)
        {
            removeEntries[i].timer -= Time.deltaTime;
            if (removeEntries[i].timer <= 0)
            {
                takenSpawns.Remove(removeEntries[i].position);
                removeEntries.RemoveAt(i);
            }
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

        Vector3 spawnPosition = GetRandomPosition(prefab.tag);
        if (spawnPosition == Vector3.zero) // If it chose plant, and there are no more spawns
            return;

        GameObject enemyObj = Instantiate(prefab, spawnLocation.position, Quaternion.identity);

        // Error check
        if (spawnPosition == Vector3.zero)
        {
            Debug.LogWarning("Couldn't find a good spawn point...");

            // Replace with pool?
            Destroy(enemyObj);
            return;
        }

        if (enemyObj.CompareTag("Plant"))
        {
            PlantController plant = enemyObj.GetComponent<PlantController>();

            plant.transform.position = spawnPosition;
            plant.playerT = player;
        }
        else
        {
            EnemyController enemy = enemyObj.GetComponent<EnemyController>();
        
            // Set position and enable
            enemy.agent.Warp(spawnPosition);
            enemy.playerT = player;
            enemy.agent.enabled = true;
        }
    
        numEnemies++;
    }

    void BuildPlantSpawns()
    {
        int numSuccess = 0;
        int numSpawns = 0;

        foreach (Transform child in plantSpawnsParent.transform)
        {
            numSpawns++;

            child.gameObject.SetActive(false);

            NavMeshHit hit;
            bool positionFound = NavMesh.SamplePosition(child.position, out hit, 1f, NavMesh.AllAreas);

            if (positionFound)
            {
                plantSpawns.Add(hit.position);
                numSuccess++;
            }
        }

        if (numSuccess != numSpawns)
        {
            Debug.LogWarning($"Found {numSuccess} / {numSpawns} spawns for plant");
        }
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

    List<float> distances = new List<float>();

    Vector3 GetRandomPosition(string objectTag)
    {
        if (objectTag == "Plant")
        {
            if (takenSpawns.Count == plantSpawns.Count)
                return Vector3.zero;

            distances.Clear();

            // Get distances
            foreach (Vector3 spawnPos in plantSpawns)
            {
                distances.Add(Vector3.Distance(spawnPos, player.position));
            }

            // Get 5 closest 
            for (int i = 0; i < 5; i++)
            {
                float king = distances[i];
                int kingIndex = i;

                for (int j = i + 1; j < distances.Count; j++)
                {
                    if (king > distances[j])
                    {
                        king = distances[j];
                        kingIndex = j;
                    }
                }

                if (kingIndex != i)
                {
                    float temp = distances[kingIndex];
                    Vector3 tempV = plantSpawns[kingIndex];

                    distances[kingIndex] = distances[i];
                    distances[i] = temp;

                    plantSpawns[kingIndex] = plantSpawns[i];
                    plantSpawns[i] = tempV;
                }
            }

            Vector3 position;
            int step = 0;

            do
            {
                position = plantSpawns[step];
                step++;
            } while (takenSpawns.Contains(position) == true && step < plantSpawns.Count);
            
            takenSpawns.Add(position);
            return position;
        }

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

    public void StopSpawning()
    {
        spawning = false;
    }
    
    IEnumerator Spawning()
    {
        while (spawning)
        {
            SpawnEnemy();
            
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void PlantKilled(Vector3 pos)
    {
        RemoveEntry entry = new RemoveEntry();
        entry.position = pos;
        entry.timer = plantRespawnCooldown;

        removeEntries.Add(entry);
    }
}
