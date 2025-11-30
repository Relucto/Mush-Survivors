using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private Queue<GameObject> pool = new();
    private GameObject prefab;
    private int maxSize;
    private int numCreated;
    Transform spawnParent;

    public Pool(GameObject _prefab, int _startSize, int _maxSize, Transform parent)
    {
        prefab = _prefab;
        maxSize = _maxSize;
        numCreated = 0;
        spawnParent = parent;

        if (_startSize > maxSize)
            maxSize = _startSize;

        GameObject[] objects = new GameObject[_startSize];
        for (int i = 0; i < _startSize; i++)
        {
            objects[i] = Get();
        }
        for (int i = 0; i < _startSize; i++)
        {
            Return(objects[i]);
        }
    }

    public GameObject Get()
    {
        //If the pool is empty
        if (pool.Count < 1)
        {
            return CreateNewObject();
        }

        //Get object from queue
        GameObject obj = pool.Dequeue();

        obj.SetActive(true);

        return obj;
    }

    public void Return(GameObject obj)
    {
        if (obj == null)
            return;
            
        if (numCreated > maxSize)
        {
            DestroyObject(obj);
        }
        else
        {
            pool.Enqueue(obj);
            obj.SetActive(false);
        }
    }

    GameObject CreateNewObject()
    {
        //Instantiate
        GameObject obj = Object.Instantiate(prefab, spawnParent);
        numCreated++;

        return obj;
    }

    void DestroyObject(GameObject obj)
    {
        Object.Destroy(obj);
        numCreated--;
    }
}
