using TMPro;
using UnityEngine;

namespace Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("Panels")] [SerializeField] private GameObject turretShopPanel;
        [SerializeField] private GameObject towerBaseUIPanel;

        [Header("Text")] [SerializeField] private TextMeshProUGUI upgradeText;

        [SerializeField] private TextMeshProUGUI sellText;
        [SerializeField] private TextMeshProUGUI turretLevelText;
        [SerializeField] private TextMeshProUGUI totalCoinsText;
        [SerializeField] private TextMeshProUGUI lifesText;
        [SerializeField] private TextMeshProUGUI currentWaveText;

        private TowerBase.TowerBase _currentTowerBaseSelected;

        private void Update()
        {
            totalCoinsText.text = CurrencyManager.Instance.TotalCoins.ToString();
            lifesText.text = LevelManager.Instance.TotalLives.ToString();
            currentWaveText.text = $"Wave: {LevelManager.Instance.CurrentWave}";
        }

        private void ShowTowerBaseUI()
        {
            towerBaseUIPanel.SetActive(true);
            UpdateUpgradeText();
            UpdateTurretLevel();
            UpdateSellValue();
        }

        public void CloseTurretShopPanel()
        {
            turretShopPanel.SetActive(false);
            CloseTurretInfoUI();
        }

        public void UpgradeTurret()
        {
            _currentTowerBaseSelected.Turret.TurretUpgrade.UpgradeTurret();
            UpdateUpgradeText();
            UpdateTurretLevel();
            UpdateSellValue();
        }

        public void SellTurret()
        {
            _currentTowerBaseSelected.SellTurret();
            _currentTowerBaseSelected = null;
            CloseTowerBaseUI();
        }

        private void UpdateUpgradeText()
        {
            upgradeText.text = _currentTowerBaseSelected.Turret.TurretUpgrade.UpgradeCost.ToString();
        }

        private void UpdateTurretLevel()
        {
            turretLevelText.text = $"Level {_currentTowerBaseSelected.Turret.TurretUpgrade.Level}";
        }

        private void UpdateSellValue()
        {
            int sellAmount = _currentTowerBaseSelected.Turret.TurretUpgrade.GetSellValue();
            sellText.text = sellAmount.ToString();
        }

        public void CloseTowerBaseUI()
        {
            towerBaseUIPanel.SetActive(false);
            if (_currentTowerBaseSelected != null)
            {
                CloseTurretInfoUI();
            }
        }

        private void CloseTurretInfoUI()
        {
            if (_currentTowerBaseSelected != null)
            {
                _currentTowerBaseSelected.CloseTurretInfo();
            }
        }

        private void TowerBaseSelected(TowerBase.TowerBase towerBaseSelected)
        {
            CloseTurretShopPanel();
            CloseTurretInfoUI();
            CloseTowerBaseUI();
            _currentTowerBaseSelected = towerBaseSelected;

            if (Camera.main != null)
            {
                Vector3 worldPosition = _currentTowerBaseSelected.transform.position;
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
                RectTransform panelRectTransform;

                if (_currentTowerBaseSelected.IsEmpty())
                {
                    turretShopPanel.SetActive(true);
                    panelRectTransform = turretShopPanel.GetComponent<RectTransform>();
                }
                else
                {
                    ShowTowerBaseUI();
                    panelRectTransform = towerBaseUIPanel.GetComponent<RectTransform>();
                }

                panelRectTransform.position = screenPosition;
            }
        }

        private void OnEnable()
        {
            TowerBase.TowerBase.OnBaseSelected += TowerBaseSelected;
        }

        private void OnDisable()
        {
            TowerBase.TowerBase.OnBaseSelected -= TowerBaseSelected;
        }
    }
}