using SpawnSystem;
using UnityEngine;

namespace Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private int lives = 10;

        public int TotalLives {get; private set;}
        public int CurrentWave {get; private set;}

        private void Awake()
        {
            TotalLives = lives;
            CurrentWave = 1;
        }
        
        private void WaveCompleted()
        {
            CurrentWave++;
        }
        
        private void OnEnable()
        {
            Enemy.Enemy.OnEndReached += ReduceLives;
            Spawner.OnWaveCompleted += WaveCompleted;
        }

        private void OnDisable()
        {
            Enemy.Enemy.OnEndReached -= ReduceLives;
            Spawner.OnWaveCompleted -= WaveCompleted;
        }

        private void ReduceLives(Enemy.Enemy enemy)
        {
            TotalLives--;
            if (TotalLives <= 0)
            {
                TotalLives = 0;
                //Game Over
            }
        }
    }
}
