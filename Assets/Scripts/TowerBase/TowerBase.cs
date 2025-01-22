using System;
using Managers;
using TurretNS;
using UnityEngine;

namespace TowerBase
{
    public class TowerBase : MonoBehaviour
    {
        public static Action<TowerBase> OnBaseSelected;
        public static Action OnTurretSold;
        public Turret Turret { get; private set; }

        public void SetTurret(Turret turret)
        {
            Turret = turret;
        }

        public bool IsEmpty()
        {
            return Turret == null;
        }

        public void SelectTurret()
        {
            OnBaseSelected?.Invoke(this);
        }

        public void SellTurret()
        {
            if (!IsEmpty())
            {
                CurrencyManager.Instance.AddCoins(Turret.TurretUpgrade.UpgradeCost);
                Destroy(Turret.gameObject);
                Turret = null;
                OnTurretSold?.Invoke();
            }
        }
    }
}
