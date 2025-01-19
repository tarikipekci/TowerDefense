using System.Collections;
using UnityEngine;
using WaypointSystem;

namespace Enemy
{
    public class EnemyAnimations : MonoBehaviour
    {
        private static readonly int Hurt = Animator.StringToHash("Hurt");
        private static readonly int Die = Animator.StringToHash("Die");
        private Animator _animator;
        private global::Enemy.Enemy _enemy;
        private EnemyHealth _enemyhealth;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _enemy = GetComponent<global::Enemy.Enemy>();
            _enemyhealth = GetComponent<EnemyHealth>();
        }

        private float GetCurrentAnimationLength()
        {
            float animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
            return animationLength;
        }
    
        private void PlayHurtAnimation()
        {
            _animator.SetTrigger(Hurt);
        }

        private void PlayDieAnimation()
        {
            _animator.SetTrigger(Die);
        }

        private IEnumerator PlayHurt()
        {
            _enemy.StopMovement();
            PlayHurtAnimation();
            yield return new WaitForSeconds(GetCurrentAnimationLength() + 0.3f);
            _enemy.ResumeMovement();
        }
    
        private void EnemyHit(global::Enemy.Enemy enemy)
        {
            if (_enemy == enemy)
            {
                StartCoroutine(PlayHurt());
            }
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
    
        private void EnemyDead(global::Enemy.Enemy enemy)
        {
            if (_enemy == enemy)
            {
                StartCoroutine(PlayDead());
            }
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
