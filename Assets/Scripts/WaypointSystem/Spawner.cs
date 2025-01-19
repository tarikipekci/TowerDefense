using System.Collections;
using Enemy;
using UnityEngine;

namespace WaypointSystem
{
    public enum SpawnModes
    {
        Fixed,
        Random
    }

    public class Spawner : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private SpawnModes spawnMode = SpawnModes.Fixed;
        [SerializeField] private int enemyCount = 10;
        [SerializeField] private float delayBtwWaves = 1f;

        [Header("Fixed Delay")] 
        [SerializeField]
        private float delayBtwSpawns;
    
        [Header("Random Delay")] 
        [SerializeField] private float minRandomDelay;
        [SerializeField] private float maxRandomDelay;

        private float _spawnTimer;
        private int _enemiesSpawned;
        private int _enemiesRemaining;

        private ObjectPooler _pooler;
        private Waypoint _waypoint;
    
        private void Start()
        {
            _pooler = GetComponent<ObjectPooler>();
            _waypoint = GetComponentInParent<Waypoint>();
            _enemiesRemaining = enemyCount;
        }

        private void Update()
        {
            _spawnTimer -= Time.deltaTime;

            if (_spawnTimer < 0)
            {
                _spawnTimer = GetSpawnDelay();
                if (_enemiesSpawned < enemyCount)
                {
                    _enemiesSpawned++;
                    SpawnEnemy();
                }
            }
        }

        private void SpawnEnemy()
        {
            GameObject newInstance = _pooler.GetInstanceFromPool();
            Enemy.Enemy enemy = newInstance.GetComponent<Enemy.Enemy>();
            enemy.Waypoint = _waypoint;
            enemy.ResetEnemy();

            enemy.transform.position = enemy.Waypoint.Points[0];
            newInstance.SetActive(true);
        }

        private float GetSpawnDelay()
        {
            float delay = 0f;

            if (spawnMode == SpawnModes.Fixed)
            {
                delay = delayBtwSpawns;
            }
            else
            {
                delay = GetRandomDelay();
            }
        
            return delay;
        }

        private float GetRandomDelay()
        {
            float randomTimer = Random.Range(minRandomDelay, maxRandomDelay);
            return randomTimer;
        }

        private IEnumerator NextWave()
        {
            yield return new WaitForSeconds(delayBtwWaves);
            _enemiesRemaining = enemyCount;
            _spawnTimer = 0f;
            _enemiesSpawned = 0;
        }
    
        private void RecordEnemy(Enemy.Enemy enemy)
        {
            _enemiesRemaining--;
            if (_enemiesRemaining <= 0)
            {
                StartCoroutine(NextWave());
            }
        }

        private void OnEnable()
        {
            Enemy.Enemy.OnEndReached += RecordEnemy;
            EnemyHealth.OnEnemyKilled += RecordEnemy;
        }

        private void OnDisable()
        {
            Enemy.Enemy.OnEndReached -= RecordEnemy;
            EnemyHealth.OnEnemyKilled -= RecordEnemy;
        }
    }
}