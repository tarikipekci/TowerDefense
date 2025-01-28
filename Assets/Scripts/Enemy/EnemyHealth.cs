using System;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        public static Action<Enemy> OnEnemyKilled;
        public static Action<Enemy> OnEnemyHit;

        [SerializeField] private GameObject healthBarPrefab;
        [SerializeField] private Transform barPosition;
        [SerializeField] private float maxHealth;

        public float CurrentHealth { get; private set; }
    
        private Image _healthBar;
        private Enemy _enemy;

        private void Start()
        {
            CreateHealthBar();
            CurrentHealth = maxHealth;
            _enemy = GetComponent<Enemy>();
        }

        private void Update()
        {
            _healthBar.fillAmount = Mathf.Lerp(_healthBar.fillAmount, CurrentHealth / maxHealth, Time.deltaTime * 10f);
        }

        private void CreateHealthBar()
        {
            GameObject newBar = Instantiate(healthBarPrefab, barPosition.position, Quaternion.identity);
            newBar.transform.SetParent(transform);
            EnemyHealthContainer container = newBar.GetComponent<EnemyHealthContainer>();
            _healthBar = container.FillAmountImage;
        }

        public void DealDamage(float damageReceived)
        {
            CurrentHealth -= damageReceived;
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Die();
            }
            else
            {
                OnEnemyHit?.Invoke(_enemy);
            }
        }

        private void Die()
        {
            OnEnemyKilled?.Invoke(_enemy);
        }

        public void ResetHealth()
        {
            CurrentHealth = maxHealth;
            _healthBar.fillAmount = 1f;
        }
    }
}
