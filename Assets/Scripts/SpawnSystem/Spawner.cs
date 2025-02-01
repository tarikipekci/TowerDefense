using System;
using System.Collections;
using Enemy;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpawnSystem
{
    public enum SpawnMode
    {
        Fixed,
        Random
    }

    public class Spawner : MonoBehaviour
    {
        public static Action OnWaveCompleted;

        [Header("Spawn Settings")]
        [SerializeField] private SpawnMode spawnMode = SpawnMode.Fixed;
        [SerializeField] private float delayBetweenWaves = 2f;

        [Header("Fixed Delay")]
        [SerializeField] private float fixedSpawnDelay = 1f;

        [Header("Random Delay")]
        [SerializeField] private float minRandomDelay = 0.5f;
        [SerializeField] private float maxRandomDelay = 2f;

        private float _spawnTimer;
        private int _enemiesSpawned;
        private int _enemiesRemaining;
        private int _currentWaveIndex;
        private int _currentWaveInfoIndex;
        private int _totalEnemiesInWaveInfo;

        private ObjectPooler _pooler;
        private bool _isWaveCompleted;

        private void Start()
        {
            InitializeComponents();
            InitializeWave();
        }

        private void InitializeComponents()
        {
            _pooler = GetComponent<ObjectPooler>();

            if (_pooler == null)
            {
                Debug.LogError("Required component is missing!");
                enabled = false;
            }
        }

        private void InitializeWave()
        {
            _currentWaveIndex = LevelManager.Instance.CurrentWave - 1;
            _currentWaveInfoIndex = 0;
            UpdateWaveInfo();
            _spawnTimer = GetSpawnDelay();
        }

        private void Update()
        {
            if (_enemiesSpawned >= _totalEnemiesInWaveInfo) return;

            _spawnTimer -= Time.deltaTime;

            if (_spawnTimer <= 0)
            {
                _spawnTimer = GetSpawnDelay();
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            var currentWave = WaveManager.Instance.waves[_currentWaveIndex];
            var waveInfo = currentWave.waveInfo[_currentWaveInfoIndex];

            GameObject enemyInstance = _pooler.GetInstanceFromPool(waveInfo.enemyPrefab);
            if (enemyInstance == null)
            {
                Debug.LogWarning("Enemy instance could not be retrieved from the pool!");
                return;
            }

            Enemy.Enemy enemy = enemyInstance.GetComponent<Enemy.Enemy>();
            if (enemy == null)
            {
                Debug.LogWarning("Enemy component is missing on the spawned object!");
                return;
            }

            enemy.SplineContainer = waveInfo.splineContainer;
            enemy.ResetEnemy();

            if (waveInfo.splineContainer != null)
            {
                enemy.transform.position = waveInfo.splineContainer.EvaluatePosition(0f);
            }
            else
            {
                Debug.LogWarning("SplineContainer is missing for this wave!");
            }

            enemyInstance.SetActive(true);
            _enemiesSpawned++;

            Debug.Log($"Enemy Spawned: {enemy.name}, Enemies Spawned: {_enemiesSpawned}");
        }

        private float GetSpawnDelay()
        {
            return spawnMode == SpawnMode.Fixed ? fixedSpawnDelay : Random.Range(minRandomDelay, maxRandomDelay);
        }

        private void HandleEnemyDefeated(Enemy.Enemy defeatedEnemy)
        {
            if (defeatedEnemy.IsDefeated) return;

            defeatedEnemy.MarkAsDefeated();
            
            _enemiesRemaining--;
            Debug.Log($"Enemy Defeated: {defeatedEnemy.name}, Enemies Remaining: {_enemiesRemaining}");

            if (_enemiesRemaining <= 0 && _enemiesSpawned >= _totalEnemiesInWaveInfo)
            {
                if (_currentWaveInfoIndex < WaveManager.Instance.waves[_currentWaveIndex].waveInfo.Count - 1)
                {
                    Debug.Log("Preparing Next Wave Info...");
                    StartCoroutine(PrepareNextWaveInfo());
                }
                else
                {
                    Debug.Log("Completing Wave...");
                    StartCoroutine(CompleteWave());
                }
            }
        }

        private void UpdateWaveInfo()
        {
            var currentWave = WaveManager.Instance.waves[_currentWaveIndex];
            _totalEnemiesInWaveInfo = currentWave.waveInfo[_currentWaveInfoIndex].count;
            _enemiesRemaining = _totalEnemiesInWaveInfo;
            _enemiesSpawned = 0;
            _spawnTimer = 0f;

            Debug.Log($"Wave Info Updated: Wave {_currentWaveIndex + 1}, Wave Info {_currentWaveInfoIndex + 1}, " +
                      $"Total Enemies: {_totalEnemiesInWaveInfo}, Enemies Remaining: {_enemiesRemaining}");
        }

        private IEnumerator PrepareNextWaveInfo()
        {
            yield return new WaitForSeconds(delayBetweenWaves);

            if (_currentWaveInfoIndex < WaveManager.Instance.waves[_currentWaveIndex].waveInfo.Count - 1)
            {
                _currentWaveInfoIndex++;
                Debug.Log($"Preparing Next Wave Info: Current Wave Info Index: {_currentWaveInfoIndex}");
            }

            UpdateWaveInfo();
        }

        private IEnumerator CompleteWave()
        {
            if (_isWaveCompleted) yield break;

            _isWaveCompleted = true;
            yield return new WaitForSeconds(delayBetweenWaves);

            if (_currentWaveInfoIndex >= WaveManager.Instance.waves[_currentWaveIndex].waveInfo.Count - 1)
            {
                _currentWaveIndex++;
                _currentWaveInfoIndex = 0;
                Debug.Log($"Wave Completed. Moving to Wave: {_currentWaveIndex + 1}");
            }

            OnWaveCompleted?.Invoke();
            UpdateWaveInfo();
            _isWaveCompleted = false;
        }

        private void OnEnable()
        {
            Enemy.Enemy.OnEndReached += HandleEnemyDefeated;
            EnemyHealth.OnEnemyKilled += HandleEnemyDefeated;
            Debug.Log("Event Subscriptions Enabled");
        }

        private void OnDisable()
        {
            Enemy.Enemy.OnEndReached -= HandleEnemyDefeated;
            EnemyHealth.OnEnemyKilled -= HandleEnemyDefeated;
            Debug.Log("Event Subscriptions Disabled");
        }
    }
}