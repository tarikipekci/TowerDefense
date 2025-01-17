using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public static Action<Enemy> OnEnemyKilled;
    public static Action<Enemy> OnEnemyHit;

    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Transform barPosition;
    [SerializeField] private float initialHealth = 10f;
    [SerializeField] private float maxHealth = 10f;

    private float CurrentHealth { get; set; }
    
    private Image _healthBar;
    private Enemy _enemy;

    private void Start()
    {
        CreateHealthBar();
        CurrentHealth = initialHealth;
        _enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            DealDamage(5f);
        }

        _healthBar.fillAmount = Mathf.Lerp(_healthBar.fillAmount, CurrentHealth / maxHealth, Time.deltaTime * 10f);
    }

    private void CreateHealthBar()
    {
        GameObject newBar = Instantiate(healthBarPrefab, barPosition.position, Quaternion.identity);
        newBar.transform.SetParent(transform);
        EnemyHealthContainer container = newBar.GetComponent<EnemyHealthContainer>();
        _healthBar = container.FillAmountImage;
    }

    private void DealDamage(float damageReceived)
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
        ObjectPooler.ReturnToPool(_enemy.gameObject);
    }

    public void ResetHealth()
    {
        CurrentHealth = initialHealth;
    }
}
