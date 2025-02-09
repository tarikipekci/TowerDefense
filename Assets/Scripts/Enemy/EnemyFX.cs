using Managers;
using TMPro;
using UnityEngine;

namespace Enemy
{
    public class EnemyFX : MonoBehaviour
    {
        [SerializeField] private Transform textDamageSpawnPosition;
        [SerializeField] private GameObject textDamagePrefab;

        private Enemy _enemy;

        private void Start()
        {
            _enemy = GetComponent<Enemy>();
        }

        private void EnemyHit(Enemy enemy, float damage)
        {
            if (_enemy == enemy)
            {
                GameObject newInstance = DamageTextManager.Instance.Pooler.GetInstanceFromPool(textDamagePrefab);
                TextMeshProUGUI damageText = newInstance.GetComponent<DamageText>().DmgText;
                damageText.text = damage.ToString();
                
                newInstance.transform.SetParent(textDamageSpawnPosition);
                newInstance.transform.position = textDamageSpawnPosition.position;
                newInstance.SetActive(true);
            }
        }
        
        private void OnEnable()
        {
            Projectile.OnEnemyHit += EnemyHit;
        }

        private void OnDisable()
        {
            Projectile.OnEnemyHit -= EnemyHit;
        }
    }
}
