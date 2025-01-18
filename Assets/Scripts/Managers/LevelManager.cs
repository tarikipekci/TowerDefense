using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private int lives = 10;

        private int TotalLives {get; set;}

        private void Start()
        {
            TotalLives = lives;
        }

        private void OnEnable()
        {
            Enemy.Enemy.OnEndReached += ReduceLives;
        }

        private void OnDisable()
        {
            Enemy.Enemy.OnEndReached -= ReduceLives;
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
