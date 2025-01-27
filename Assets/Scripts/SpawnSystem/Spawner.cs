using System;
using System.Collections;
using Enemy;
using Managers;
using UnityEngine;
using WaypointSystem;
using Random = UnityEngine.Random;

namespace SpawnSystem
{
    public enum SpawnModes
    {
        Fixed,
        Random
    }

    public class Spawner : MonoBehaviour
    {
        public static Action OnWaveCompleted;

        [Header("Settings")] [SerializeField] private SpawnModes spawnMode = SpawnModes.Fixed;
        [SerializeField] private float delayBtwWaves = 2f;

        [Header("Fixed Delay")] [SerializeField]
        private float delayBtwSpawns;

        [Header("Random Delay")] [SerializeField]
        private float minRandomDelay;

        [SerializeField] private float maxRandomDelay;

        private float _spawnTimer;
        private int _enemiesSpawned;
        private int _enemiesRemaining;
        private int _waveInfoCount;
        private int _currentWaveInfoIndex;
        private int _enemyCount;

        private ObjectPooler _pooler;
        private Waypoint _waypoint;

        private void Start()
        {
            _pooler = GetComponent<ObjectPooler>();
            _waypoint = GetComponentInParent<Waypoint>();
            _waveInfoCount = WaveManager.Instance.waves[LevelManager.Instance.CurrentWave - 1].waveInfo.Count;
            _enemiesRemaining = WaveManager.Instance.waves[LevelManager.Instance.CurrentWave - 1]
                .waveInfo[_currentWaveInfoIndex].count;
            _enemyCount = _enemiesRemaining;
        }

        private void Update()
        {
            _spawnTimer -= Time.deltaTime;

            if (_spawnTimer <= 0)
            {
                _spawnTimer = GetSpawnDelay();
                if (_enemiesSpawned < _enemyCount)
                {
                    _enemiesSpawned++;
                    SpawnEnemy();
                }
            }
        }
        
        private void SpawnEnemy()
        {
            var currentWaveCounter = LevelManager.Instance.CurrentWave;
            var currentWave = WaveManager.Instance.waves[currentWaveCounter - 1];
            var currentWaveInfo = currentWave.waveInfo;
            GameObject newInstance = _pooler.GetInstanceFromPool(currentWaveInfo[_currentWaveInfoIndex].enemyPrefab);
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

        private IEnumerator NextWaveInfo(bool isWaveFinished)
        {
            yield return new WaitForSeconds(delayBtwWaves);
            
            if (isWaveFinished)
            {
                OnWaveCompleted?.Invoke();
                _currentWaveInfoIndex = 0;
                ResetWaveInfo();
            }
            else
            {
                _currentWaveInfoIndex++;
                ResetWaveInfo();
            }
        }

        private void ResetWaveInfo()
        {
            _spawnTimer = 0f;
            _enemiesSpawned = 0;
            _enemiesRemaining = WaveManager.Instance.waves[LevelManager.Instance.CurrentWave - 1]
                .waveInfo[_currentWaveInfoIndex].count;
            _waveInfoCount = WaveManager.Instance.waves[LevelManager.Instance.CurrentWave - 1].waveInfo.Count;
            _enemyCount = _enemiesRemaining;
        }
        
        private void RecordEnemy(Enemy.Enemy enemy)
        {
            _enemiesRemaining--;
            if (_enemiesRemaining <= 0)
            {
                if (_currentWaveInfoIndex < _waveInfoCount - 1)
                {
                    StartCoroutine(NextWaveInfo(false));   
                }
                else
                {
                    StartCoroutine(NextWaveInfo(true));
                }
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