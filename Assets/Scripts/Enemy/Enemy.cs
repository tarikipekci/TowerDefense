using System;
using SpawnSystem;
using UnityEngine;
using WaypointSystem;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        public static Action<Enemy> OnEndReached;
    
        [SerializeField] private float moveSpeed = 3f;
        private float MoveSpeed { get; set; }
        public EnemyHealth EnemyHealth { get; private set; }
        
        public bool IsDefeated { get; private set; }
    
        private int _currentWaypointIndex;
        private Vector3 _lastPointPosition;
    
        private EnemyHealth _enemyHealth;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody2D;

        public Waypoint Waypoint { get; set; }
        public Vector3 CurrentPointPosition => Waypoint.GetWaypointPosition(_currentWaypointIndex);

        private void Start()
        {
            _enemyHealth = GetComponent<EnemyHealth>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            EnemyHealth = GetComponent<EnemyHealth>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _currentWaypointIndex = 0;
            MoveSpeed = moveSpeed;
            _lastPointPosition = transform.position;
        }

        private void FixedUpdate()
        {
            Move();
            Rotate();
        
            if (CurrentPointPositionReached())
            {
                UpdateCurrentPointIndex();
            }
        }

        private void Move()
        {
            Vector3 direction = (CurrentPointPosition - transform.position).normalized;
            _rigidbody2D.velocity = direction * MoveSpeed;
        }

        public void StopMovement()
        {
            MoveSpeed = 0f;
        }

        public void ResumeMovement()
        {
            MoveSpeed = moveSpeed;
        }

        private void Rotate()
        {
            // Horizontal Left-Right
            if (CurrentPointPosition.x > _lastPointPosition.x)
            {
                _spriteRenderer.flipX = false;
            }
            else
            {
                _spriteRenderer.flipX = true;
            }
        }
    
        private bool CurrentPointPositionReached()
        {
            float distanceToNextPointPosition = (transform.position - CurrentPointPosition).magnitude;

            if (distanceToNextPointPosition < 0.1f)
            {
                _lastPointPosition = transform.position;
                return true;
            }
            return false;
        }

        private void UpdateCurrentPointIndex()
        {
            int lastWaypointIndex = Waypoint.Points.Length - 1;
            if (_currentWaypointIndex < lastWaypointIndex)
            {
                _currentWaypointIndex++;
            }
            else
            {
                EndPointReached();
            }
        }

        private void EndPointReached()
        {
            OnEndReached?.Invoke(this);
            _enemyHealth.ResetHealth();
            ObjectPooler.ReturnToPool(gameObject);
        }

        public void ResetEnemy()
        {
            _currentWaypointIndex = 0;
            IsDefeated = false; 
        }
        
        public void MarkAsDefeated()
        {
            if (IsDefeated) return;
            IsDefeated = true;
        }

        public Rigidbody2D GetRigidbody2D()
        {
            return _rigidbody2D;
        }
    }
}
