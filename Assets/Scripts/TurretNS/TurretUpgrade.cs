using System;
using Managers;
using UnityEngine;

namespace TurretNS
{
    public class TurretUpgrade : MonoBehaviour
    {
        [SerializeField] private int upgradeInitialCost;
        [SerializeField] private int upgradeCostIncremental;
        [SerializeField] private float damageIncremental;
        [SerializeField] private float delayReduce;

        public int UpgradeCost { get; set; }

        private TurretProjectile _turretProjectile;

        private void Start()
        {
            _turretProjectile = GetComponent<TurretProjectile>();
            UpgradeCost = upgradeInitialCost;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                UpgradeTurret();
            }
        }

        private void UpgradeTurret()
        {
            if (CurrencyManager.Instance.TotalCoins >= UpgradeCost)
            {
                _turretProjectile.Damage += damageIncremental;
                _turretProjectile.DelayPerShot -= delayReduce;
                UpdateUpgrade();
            }
        }

        private void UpdateUpgrade()
        {
            CurrencyManager.Instance.RemoveCoins(UpgradeCost);
            UpgradeCost += upgradeCostIncremental;
        }
    }
}