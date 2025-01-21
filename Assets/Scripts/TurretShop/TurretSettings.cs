using UnityEngine;

namespace TurretShop
{
    [CreateAssetMenu(fileName = "Turret Shop Setting")]
    public class TurretSettings : ScriptableObject
    {
        public GameObject turretPrefab;
        public int turretShopCost;
        public Sprite turretShopIcon;
    }
}
