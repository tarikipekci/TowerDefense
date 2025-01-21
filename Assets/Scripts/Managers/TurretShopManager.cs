using Node;
using TurretNS;
using TurretShop;
using UnityEngine;

namespace Managers
{
    public class TurretShopManager : MonoBehaviour
    {
        [SerializeField] private GameObject turretCardPrefab;
        [SerializeField] private Transform turretPanelContainer;

        [Header("Turret Settings")] [SerializeField]
        private TurretSettings[] turrets;

        private TowerBase _currentTowerBaseSelected;

        private void Start()
        {
            foreach (var t in turrets)
            {
                CreateTurretCard(t);
            }
        }

        private void CreateTurretCard(TurretSettings turretSettings)
        {
            GameObject newInstance = Instantiate(turretCardPrefab, turretPanelContainer.position, Quaternion.identity);
            newInstance.transform.SetParent(turretPanelContainer);
            newInstance.transform.localScale = Vector3.one;

            TurretCard cardButton = newInstance.GetComponent<TurretCard>();
            cardButton.SetupTurretButton(turretSettings);
        }

        private void TowerBaseSelected(TowerBase towerBaseSelected)
        {
           _currentTowerBaseSelected = towerBaseSelected;
        }

        private void PlaceTurret(TurretSettings turretLoaded)
        {
            if (_currentTowerBaseSelected != null)
            {
                GameObject turretInstance = Instantiate(turretLoaded.turretPrefab);
                turretInstance.transform.localPosition = _currentTowerBaseSelected.transform.position;
                turretInstance.transform.parent = _currentTowerBaseSelected.transform;

                Turret turretPlaced = turretInstance.GetComponent<Turret>();
                _currentTowerBaseSelected.SetTurret(turretPlaced);
                _currentTowerBaseSelected.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        
        private void OnEnable()
        {
            TowerBase.OnBaseSelected += TowerBaseSelected;
            TurretCard.OnPlaceTurret += PlaceTurret;
        }

        private void OnDisable()
        {
            TowerBase.OnBaseSelected -= TowerBaseSelected;
            TurretCard.OnPlaceTurret -= PlaceTurret;
        }
    }
}
