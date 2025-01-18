using UnityEngine;

namespace Managers
{
    public class DamageTextManager : MonoBehaviour
    {
        public ObjectPooler Pooler { get; set; }

        public static DamageTextManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Pooler = GetComponent<ObjectPooler>();
        }
    }
}