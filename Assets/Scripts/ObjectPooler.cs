using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 10;
    private List<GameObject> _pool;
    private GameObject _poolContainer;

    private void Awake()
    {
        _pool = new List<GameObject>();
        _poolContainer = new GameObject($"Pool - {prefab.name}");
        CreatePooler();
    }

    private void CreatePooler()
    {
        for (int i = 0; i < poolSize; i++)
        {
            _pool.Add(CreateInstance());
        }
    }

    private GameObject CreateInstance()
    {
        GameObject newInstance = Instantiate(prefab, _poolContainer.transform);
        newInstance.SetActive(false);
        return newInstance;
    }

    public GameObject GetInstanceFromPool()
    {
        foreach (var t in _pool)
        {
            if (!t.activeInHierarchy)
            {
                return t;
            }
        }

        return CreateInstance();
    }

    public static void ReturnToPool(GameObject instance)
    {
        instance.SetActive(false);
    }

    public static IEnumerator ReturnToPoolWithDelay(GameObject instance, float delay)
    {
        yield return new WaitForSeconds(delay);
        instance.SetActive(false);
    }
}
