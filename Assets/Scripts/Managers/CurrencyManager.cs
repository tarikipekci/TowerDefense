using Enemy;
using UnityEngine;

namespace Managers
{
    public class CurrencyManager : Singleton<CurrencyManager>
    {
        [SerializeField] private int coinTest;
        private const string CURRENCY_SAVE_KEY = "MYGAME_CURRENCY";

        public int TotalCoins { get; private set; }

        private void Start()
        {
            PlayerPrefs.DeleteKey(CURRENCY_SAVE_KEY);
            LoadCoins();
        }

        private void LoadCoins()
        {
            TotalCoins = PlayerPrefs.GetInt(CURRENCY_SAVE_KEY, coinTest);
        }

        private void AddCoins(int amount)
        {
            TotalCoins += amount;
            PlayerPrefs.SetInt(CURRENCY_SAVE_KEY, TotalCoins);
            PlayerPrefs.Save();
        }

        public void RemoveCoins(int amount)
        {
            if (TotalCoins >= amount)
            {
                TotalCoins -= amount; 
                PlayerPrefs.SetInt(CURRENCY_SAVE_KEY, TotalCoins);
                PlayerPrefs.Save();
            }
        }

        private void AddCoins(Enemy.Enemy enemy)
        {
            AddCoins(1);
        }
        
        private void OnEnable()
        {
            EnemyHealth.OnEnemyKilled += AddCoins;
        }

        private void OnDisable()
        {
            EnemyHealth.OnEnemyKilled -= AddCoins;
        }
    }
}
