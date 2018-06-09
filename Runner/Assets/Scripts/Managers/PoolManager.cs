using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {
    Dictionary<int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();

    static PoolManager _instance;

    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PoolManager>();
            }
            return _instance;
        }
    }

    public void CreatePool(GameObject prefab, int poolSize)
    {
        int poolKey = prefab.GetInstanceID();

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<GameObject>());
            GameObject parent = GameObject.Find(prefab.name);

            if (parent == null)
            {
                parent = new GameObject(prefab.name + "Pool");
                parent.transform.parent = transform;
            }

            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab) as GameObject;
                obj.SetActive(false);
                obj.transform.parent = parent.transform;
                obj.GetComponent<PoolObject>().SetPoolKey(poolKey);
                poolDictionary[poolKey].Enqueue(obj);
            }
        }
    }

    public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();
        if (poolDictionary.ContainsKey(poolKey))
        {
            GameObject obj = poolDictionary[poolKey].Dequeue();
            obj.SetActive(true);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.GetComponent<PoolObject>().OnObjectReuse();
        }
    }

    public void ReuseObject(int poolKey)
    {
        if (poolDictionary.ContainsKey(poolKey))
        {
            GameObject obj = poolDictionary[poolKey].Dequeue();
            obj.SetActive(true);
            obj.GetComponent<PoolObject>().OnObjectReuse();
        }
    }

    public void EnQueue(GameObject obj)
    {
        poolDictionary[obj.GetComponent<PoolObject>().GetPoolKey()].Enqueue(obj);
    }
}
