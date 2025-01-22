using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string name;
        public GameObject prefab;
        public int size;
    }

    [Header("Pools Configuration")]
    [SerializeField] private Pool[] pools;

    private Dictionary<string, Queue<GameObject>> poolDictionary;
    [SerializeField] RSO_PoolManager rsoPoolManager;

    private void Awake()
    {
        rsoPoolManager.Value = this;
        InitializePools();
    }

    private void InitializePools()
    {
        if (pools == null || pools.Length == 0)
        {
            Debug.LogError("Pools configuration is invalid or empty.");
            return;
        }

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            if (string.IsNullOrEmpty(pool.name))
            {
                Debug.LogError("Pool has an empty or null name.");
                continue;
            }

            if (pool.prefab == null)
            {
                Debug.LogError("Pool with name " + pool.name + " has a null prefab.");
                continue;
            }

            if (pool.size <= 0)
            {
                Debug.LogWarning("Pool with name " + pool.name + " has a size of 0 or less. Defaulting to 1.");
                pool.size = 1;
            }

            var objectQueue = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectQueue.Enqueue(obj);

                // Add ParticleSystem handling
                var particleSystem = obj.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    var psAutoReturn = obj.AddComponent<ParticleSystemAutoReturn>();
                    psAutoReturn.Initialize(this, pool.name);
                }
            }

            poolDictionary.Add(pool.name, objectQueue);
        }
    }

    public GameObject GetFromPool(string name, Vector3 position, Quaternion rotation)
    {
        if (poolDictionary == null || poolDictionary.Count == 0)
        {
            Debug.LogError("Pool dictionary is not initialized.");
            return null;
        }

        if (!poolDictionary.ContainsKey(name))
        {
            Debug.LogError("Pool with name " + name + " does not exist.");
            return null;
        }

        var objectQueue = poolDictionary[name];

        GameObject obj;
        if (objectQueue.Count > 0)
        {
            obj = objectQueue.Dequeue();
        }
        else
        {
            Debug.LogWarning("Pool with name " + name + " is empty. Creating a new instance.");
            var pool = System.Array.Find(pools, p => p.name == name);
            if (pool == null || pool.prefab == null)
            {
                Debug.LogError("Prefab for pool " + name + " is not defined.");
                return null;
            }

            obj = Instantiate(pool.prefab);
            var particleSystem = obj.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                var psAutoReturn = obj.AddComponent<ParticleSystemAutoReturn>();
                psAutoReturn.Initialize(this, name);
            }
        }

        ResetObject(obj);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        return obj;
    }

    public void ReturnToPool(string name, GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning("Attempted to return a null object to the pool.");
            return;
        }

        if (!poolDictionary.ContainsKey(name))
        {
            Debug.LogError("Pool with name " + name + " does not exist.");
            return;
        }

        obj.SetActive(false);
        obj.transform.SetParent(null);
        poolDictionary[name].Enqueue(obj);
    }

    private void ResetObject(GameObject obj)
    {
        foreach (var component in obj.GetComponents<MonoBehaviour>())
        {
            if (component is IResettable resettable)
            {
                resettable.ResetState();
            }
            else
            {
                var type = component.GetType();
                var method = type.GetMethod("Awake", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                method?.Invoke(component, null);

                method = type.GetMethod("Start", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                method?.Invoke(component, null);
            }
        }
    }
}

public interface IResettable
{
    void ResetState();
}

public class ParticleSystemAutoReturn : MonoBehaviour
{
    private PoolManager poolManager;
    private string poolName;

    public void Initialize(PoolManager manager, string name)
    {
        poolManager = manager;
        poolName = name;
    }

    private void OnParticleSystemStopped()
    {
        poolManager.ReturnToPool(poolName, gameObject);
    }
}