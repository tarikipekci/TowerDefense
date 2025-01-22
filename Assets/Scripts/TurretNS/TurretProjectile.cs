using UnityEngine;
using WaypointSystem;

namespace TurretNS
{
    public class TurretProjectile : MonoBehaviour
    {
        [SerializeField] protected Transform projectileSpawnPosition;
        [SerializeField] protected float delayBtwAttacks = 1f;
        [SerializeField] protected float damage = 2f;
        
        public float Damage { get; set; }
        public float DelayPerShot { get; set; }

        private float _nextAttackTime;
        private ObjectPooler _pooler;
        private Turret _turret;
        private Projectile _currentProjectileLoaded;

        private void Start()
        {
            _turret = GetComponent<Turret>();
            _pooler = gameObject.GetComponent<ObjectPooler>();
            Damage = damage;
            DelayPerShot = delayBtwAttacks;
            LoadProjectile();
        }

        private void Update()
        {
            if (IsTurretEmpty())
            {
                LoadProjectile();
            }

            if (Time.time > _nextAttackTime)
            {
                if (_turret.CurrentEnemyTarget != null && _currentProjectileLoaded != null &&
                    _turret.CurrentEnemyTarget.EnemyHealth.CurrentHealth > 0f)
                {
                    _currentProjectileLoaded.transform.parent = null;
                    _currentProjectileLoaded.SetEnemy(_turret.CurrentEnemyTarget);
                }   
            
                _nextAttackTime = Time.time + DelayPerShot;
            }
        }

        private void LoadProjectile()
        {
            GameObject newInstance = _pooler.GetInstanceFromPool();
            newInstance.transform.localPosition = projectileSpawnPosition.position;
            newInstance.transform.SetParent(projectileSpawnPosition);
            _currentProjectileLoaded = newInstance.GetComponent<Projectile>();
            _currentProjectileLoaded.TurretOwner = this;
            _currentProjectileLoaded.ResetProjectile();
            _currentProjectileLoaded.Damage = Damage;
            newInstance.SetActive(true);
            _currentProjectileLoaded.GetComponent<SpriteRenderer>().enabled = false;
        }

        private bool IsTurretEmpty()
        {
            return _currentProjectileLoaded == null;
        }
    
        public void ResetTurretProjectile()
        {
            _currentProjectileLoaded = null;
        }
    }
}