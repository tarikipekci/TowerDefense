using Node;
using UnityEngine;

namespace Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("Panels")] [SerializeField] private GameObject turretShopPanel;
        
        private TowerBase _currentTowerBaseSelected;

        public void CloseTurretShopPanel()
        {
            turretShopPanel.SetActive(false);
        }
        private void TowerBaseSelected(TowerBase towerBaseSelected)
        {
            _currentTowerBaseSelected = towerBaseSelected;
            if (_currentTowerBaseSelected.IsEmpty())
            {
                turretShopPanel.SetActive(true);
                
                if (Camera.main != null)
                {
                    Vector3 worldPosition = _currentTowerBaseSelected.transform.position;
                    Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

                    RectTransform panelRectTransform = turretShopPanel.GetComponent<RectTransform>();
                    panelRectTransform.position = screenPosition;
                }
            }
        }
        
        private void OnEnable()
        {
            TowerBase.OnBaseSelected += TowerBaseSelected;
        }

        private void OnDisable()
        {
            TowerBase.OnBaseSelected -= TowerBaseSelected;
        }
    }
}
