using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour, IAwaitable
{
    public static EnemySpawner Instance { get; private set; }

    public int maxEnemies = 50;
    public float waitTime = 0.5f;
    public float plantRespawnCooldown;
    public SpawnSetting[] spawnSettings;
    [HideInInspector] public static int numEnemies = 0;
    //public GameObject enemyPrefab;
    public Transform spawnLocation, player;
    public GameObject plantSpawnsParent;

    public class RemoveEntry
    {
        public Vector3 position;
        public float timer;
    }

    List<EnemyType> enemies = new List<EnemyType>();
    List<Vector3> plantSpawns = new List<Vector3>();
    List<Vector3> takenSpawns = new List<Vector3>();
    List<RemoveEntry> removeEntries = new List<RemoveEntry>();
    NavMeshTriangulation triangulation;

    SpawnSetting activeSpawnSetting;
    float currentWaitTime, runTime;
    int totalChance;
    bool spawning, isReady;

    [System.Serializable]
    public struct EnemyType
    {
        public GameObject prefab;
        public int spawnChance;
        public float health;
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
        currentWaitTime = waitTime;
        activeSpawnSetting = spawnSettings[0];

        SetSpawns();

        BuildPlantSpawns();

        triangulation = NavMesh.CalculateTriangulation();

        isReady = true;
    }

    public bool IsReady() => isReady;

    void Update()
    {
        // Spawn enemies
        if (spawning)
        {
            runTime += Time.deltaTime;
            currentWaitTime -= Time.deltaTime;

            if (currentWaitTime <= 0)
            {
                currentWaitTime = waitTime;

                SpawnEnemy();
            }
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

        // Check round time for spawn settings
        foreach (SpawnSetting setting in spawnSettings)
        {
            // If start time is here
            if (setting.startTime < runTime)
            {
                // If this one's start time is greater than the active one
                if (setting.startTime > activeSpawnSetting.startTime)
                {
                    print(setting.startTime + " > " + activeSpawnSetting.startTime);
                    print($"Activating {setting.name}");
                    activeSpawnSetting = setting;
                    SetSpawns();
                }
            }
        }
    }

    void SetSpawns()
    {
        waitTime = activeSpawnSetting.waitTime;
        maxEnemies = activeSpawnSetting.maxEnemies;

        enemies.Clear();

        totalChance = 0;
        
        foreach (EnemyType enemy in activeSpawnSetting.enemies)
        {
            enemies.Add(enemy);

            totalChance += enemy.spawnChance;
        }
        
        print("Wait time = " + waitTime);
        print("Max enemies = " + maxEnemies);
        print("Total chance = " + totalChance);
    }

    void SpawnEnemy()
    {
        if (numEnemies >= maxEnemies)
            return;

        // Replace with pool?
        EnemyType enemyType = GetRandomEnemy();

        if (enemyType.prefab == null)
            return;

        Vector3 spawnPosition = GetRandomPosition(enemyType.prefab.tag);
        if (spawnPosition == Vector3.zero) // If it chose plant, and there are no more spawns
            return;

        GameObject enemyObj = Instantiate(enemyType.prefab, spawnLocation.position, Quaternion.identity);

        // Error check
        if (spawnPosition == Vector3.zero)
        {
            Debug.LogWarning("Couldn't find a good spawn point...");

            // Replace with pool?
            Destroy(enemyObj);
            return;
        }

        enemyObj.GetComponent<Health>().SetMaxHealth(enemyType.health);

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

    EnemyType GetRandomEnemy()
    {
        int randomValue = Random.Range(0, totalChance);

        foreach (EnemyType enemy in enemies)
        {
            if (randomValue < enemy.spawnChance)
            {
                return enemy;
            }

            randomValue -= enemy.spawnChance;
        }

        Debug.LogError("SOMETHING WENT WRONG WITH THE RANDOM SPAWN CHANCE THING HELP");
        Debug.LogError("randomValue chosen was " + randomValue + "??");
        Debug.Break();
        return new EnemyType();
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
    }

    public void StopSpawning()
    {
        spawning = false;
    }

    public void PlantKilled(Vector3 pos)
    {
        RemoveEntry entry = new RemoveEntry();
        entry.position = pos;
        entry.timer = plantRespawnCooldown;

        removeEntries.Add(entry);
    }
}
