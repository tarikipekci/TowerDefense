using System;
using TurretNS;
using UnityEngine;

namespace Node
{
    public class TowerBase : MonoBehaviour
    {
        public static Action<TowerBase> OnBaseSelected;
        public Turret Turret { get; set; }

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
    }
}
