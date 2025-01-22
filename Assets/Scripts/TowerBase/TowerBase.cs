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

        [SerializeField] private GameObject attackRangeSprite;
        public Turret Turret { get; private set; }

        private float _rangeSize;
        private Vector3 _rangeOriginalSize;

        private void Start()
        {
            _rangeSize = attackRangeSprite.GetComponent<SpriteRenderer>().bounds.size.y;
            _rangeOriginalSize = attackRangeSprite.transform.localScale;
        }

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
            if (!IsEmpty())
            {
                ShowTurretInfo();
            }
        }

        public void SellTurret()
        {
            if (!IsEmpty())
            {
                CurrencyManager.Instance.AddCoins(Turret.TurretUpgrade.GetSellValue());
                Destroy(Turret.gameObject);
                Turret = null;
                attackRangeSprite.SetActive(false);
                OnTurretSold?.Invoke();
            }
        }

        private void ShowTurretInfo()
        {
            attackRangeSprite.SetActive(true);
            attackRangeSprite.transform.localScale = _rangeOriginalSize * Turret.AttackRange / (_rangeSize / 2);
        }

        public void CloseTurretInfo()
        {
            attackRangeSprite.SetActive(false);
        }
    }
}
