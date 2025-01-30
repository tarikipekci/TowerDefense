using System.Collections;
using SpawnSystem;
using UnityEngine;

namespace Enemy
{
    public class EnemyAnimations : MonoBehaviour
    {
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Up = Animator.StringToHash("Up");
        private static readonly int Down = Animator.StringToHash("Down");
        private Animator _animator;
        private Enemy _enemy;
        private EnemyHealth _enemyhealth;
        private Vector3 previousPosition;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _enemy = GetComponent<Enemy>();
            _enemyhealth = GetComponent<EnemyHealth>();
            previousPosition = transform.position;
        }

        private void Update()
        {
            PlayUpDownAnimation();
        }

        private float GetCurrentAnimationLength()
        {
            float animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
            return animationLength;
        }
        
        private void PlayDieAnimation()
        {
            _animator.SetTrigger(Die);
        }
        
        private IEnumerator PlayDead()
        {
            _enemy.StopMovement();
            PlayDieAnimation();
            yield return new WaitForSeconds(GetCurrentAnimationLength() + 0.3f);
            _enemy.ResumeMovement();
            _enemyhealth.ResetHealth();
            ObjectPooler.ReturnToPool(_enemy.gameObject);
        }

        private void EnemyDead(Enemy enemy)
        {
            if (_enemy == enemy)
            {
                StartCoroutine(PlayDead());
            }
        }

        private void PlayUpDownAnimation()
        {
            var rb = _enemy.GetRigidbody2D();
            Vector3 currentPosition = transform.position;
            
            if (rb.velocity.y > 0.1f)
            {
                _animator.SetBool(Up, true);
                _animator.SetBool(Down, false);
            }
            else if (rb.velocity.y < -0.1f)
            {
                _animator.SetBool(Up, false);
                _animator.SetBool(Down, true);
            }
            else
            {
                _animator.SetBool(Up, false);
                _animator.SetBool(Down, false);
            }
            
            previousPosition = currentPosition;
        }

        private void OnEnable()
        {
            //EnemyHealth.OnEnemyHit += EnemyHit;
            EnemyHealth.OnEnemyKilled += EnemyDead;
        }

        private void OnDisable()
        {
            //EnemyHealth.OnEnemyHit -= EnemyHit;
            EnemyHealth.OnEnemyKilled -= EnemyDead;
        }
    }
}