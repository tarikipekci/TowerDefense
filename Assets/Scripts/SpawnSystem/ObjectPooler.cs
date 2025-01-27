using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnSystem
{
    [Serializable]
    public struct PooledObject
    {
        public GameObject prefab;
        public int count;
    }
    
    public class ObjectPooler : MonoBehaviour
    {
        [SerializeField] private List<PooledObject> pooledObjects;
        private Dictionary<GameObject, List<GameObject>> _pools;

        private void Awake()
        {
            _pools = new Dictionary<GameObject, List<GameObject>>();

            foreach (var pooledObject in pooledObjects)
            {
                var pool = new List<GameObject>();
                for (int i = 0; i < pooledObject.count; i++)
                {
                    var instance = Instantiate(pooledObject.prefab, transform);
                    instance.SetActive(false);
                    pool.Add(instance);
                }
                _pools[pooledObject.prefab] = pool;
            }
        }

        public GameObject GetInstanceFromPool(GameObject prefab)
        {
            if (_pools.TryGetValue(prefab, out var pool))
            {
                foreach (var instance in pool)
                {
                    if (!instance.activeInHierarchy)
                    {
                        instance.SetActive(true);
                        return instance;
                    }
                }

                var newInstance = Instantiate(prefab, transform);
                newInstance.SetActive(false);
                pool.Add(newInstance);
                return newInstance;
            }

            Debug.LogWarning($"Prefab '{prefab.name}' couldn't find!");
            return null;
        }

        public static void ReturnToPool(GameObject instance)
        {
            instance.SetActive(false);
        }

        public IEnumerator ReturnToPoolWithDelay(GameObject instance, float delay)
        {
            yield return new WaitForSeconds(delay);
            ReturnToPool(instance);
        }
    }
}
