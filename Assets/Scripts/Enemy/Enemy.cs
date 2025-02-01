using System;
using SpawnSystem;
using UnityEngine;
using UnityEngine.Splines;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        public static Action<Enemy> OnEndReached;

        [SerializeField] private float moveSpeed = 3f;
        private float MoveSpeed { get; set; }
        public EnemyHealth EnemyHealth { get; private set; }
        public bool IsDefeated { get; private set; }

        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;

        public SplineContainer SplineContainer { get; set; } 
        private float t; 

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            EnemyHealth = GetComponent<EnemyHealth>();

            MoveSpeed = moveSpeed;
        }

        private void FixedUpdate()
        {
            Move();
            Rotate();

            if (t >= 1f)
            {
                EndPointReached();
            }
        }

        private void Move()
        {
            if (SplineContainer == null) return;

            t += (MoveSpeed / SplineContainer.CalculateLength()) * Time.deltaTime;
            t = Mathf.Clamp01(t);

            Vector3 newPosition = SplineContainer.EvaluatePosition(t);

            Vector2 direction = new Vector2(newPosition.x, newPosition.y) - _rigidbody2D.position;

            _rigidbody2D.velocity = direction.normalized * MoveSpeed;
        }

        private void Rotate()
        {
            if (SplineContainer == null) return;

            Vector3 previousPosition = SplineContainer.EvaluatePosition(Mathf.Max(0, t - 0.01f));
            Vector3 direction = (transform.position - previousPosition).normalized;

            if (direction.x > 0)
                _spriteRenderer.flipX = false;
            else
                _spriteRenderer.flipX = true;
        }

        public void StopMovement()
        {
            MoveSpeed = 0f;
        }

        public void ResumeMovement()
        {
            MoveSpeed = moveSpeed;
        }

        private void EndPointReached()
        {
            OnEndReached?.Invoke(this);
            EnemyHealth.ResetHealth();
            ObjectPooler.ReturnToPool(gameObject);
        }

        public void ResetEnemy()
        {
            t = 0f;
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

        public float GetSplineProgress()
        {
            if (SplineContainer == null || SplineContainer.Splines.Count == 0)
                return 0f;

            Spline spline = SplineContainer.Splines[0]; 
            float closestT = 0f;
            float minDistance = float.MaxValue;
    
            for (float t = 0f; t <= 1f; t += 0.01f) 
            {
                Vector3 splinePoint = spline.EvaluatePosition(t);
                float distance = Vector3.Distance(transform.position, splinePoint);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestT = t;
                }
            }

            return closestT;
        }
    }
}
