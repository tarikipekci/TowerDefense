using System.Collections.Generic;
using UnityEngine;

namespace TurretNS
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private float attackRange = 3f;

        public Enemy.Enemy CurrentEnemyTarget { get; private set; }
        public TurretUpgrade TurretUpgrade { get; private set; }

        public float AttackRange => attackRange;

        private bool _gameStarted;
        private List<Enemy.Enemy> _enemies = new List<Enemy.Enemy>();

        private void Start()
        {
            _gameStarted = true;
            _enemies = new List<Enemy.Enemy>();
            TurretUpgrade = GetComponent<TurretUpgrade>();
        }

        private void Update()
        {
            UpdateCurrentEnemyTarget();
        }

        private void UpdateCurrentEnemyTarget()
        {
            if (_enemies.Count <= 0)
            {
                CurrentEnemyTarget = null;
                return;
            }
            
            _enemies.Sort((Enemy1, Enemy2) =>
            {
                if (Enemy1.CurrentPointPosition == Enemy2.CurrentPointPosition)
                {
                    return Vector3.Distance(transform.position, Enemy1.CurrentPointPosition)
                        .CompareTo(Vector3.Distance(transform.position, Enemy2.CurrentPointPosition));
                }

                return 0;
            });

            CurrentEnemyTarget = _enemies[0];
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                Enemy.Enemy newEnemy = other.GetComponent<Enemy.Enemy>();
                _enemies.Add(newEnemy);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                Enemy.Enemy enemy = other.GetComponent<Enemy.Enemy>();
                if (_enemies.Contains(enemy))
                {
                    _enemies.Remove(enemy);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (!_gameStarted)
            {
                GetComponent<CircleCollider2D>().radius = attackRange;
            }

            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}