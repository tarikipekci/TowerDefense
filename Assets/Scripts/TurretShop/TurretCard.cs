using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TurretShop
{
    public class TurretCard : MonoBehaviour
    {
        public static Action<TurretSettings> OnPlaceTurret;
        
        [SerializeField] private Image turretImage;
        [SerializeField] private TextMeshProUGUI turretCost;
        
        public TurretSettings TurretLoaded { get; set; }

        public void SetupTurretButton(TurretSettings turretSettings)
        {
            TurretLoaded = turretSettings;
            turretImage.sprite = turretSettings.turretShopIcon;
            turretCost.text = turretSettings.turretShopCost.ToString();
        }

        public void PlaceTurret()
        {
            if (CurrencyManager.Instance.TotalCoins >= TurretLoaded.turretShopCost)
            {
                CurrencyManager.Instance.RemoveCoins(TurretLoaded.turretShopCost);
                UIManager.Instance.CloseTurretShopPanel();
                OnPlaceTurret?.Invoke(TurretLoaded);
            }
        }
    }
}
