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

        [Header("Sell")] 
        [Range(0,1)]
        [SerializeField] private float sellPerc;

        private float SellPerc { get; set; }
        public int UpgradeCost { get; private set; }
        public int Level { get; private set; }

        private TurretProjectile _turretProjectile;

        private void Start()
        {
            _turretProjectile = GetComponent<TurretProjectile>();
            UpgradeCost = upgradeInitialCost;
            Level = 1;
            SellPerc = sellPerc;
        }
        
        public void UpgradeTurret()
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
            Level++;
        }

        public int GetSellValue()
        {
            int sellValue = Mathf.RoundToInt(UpgradeCost * SellPerc);
            return sellValue;
        }
    }
}