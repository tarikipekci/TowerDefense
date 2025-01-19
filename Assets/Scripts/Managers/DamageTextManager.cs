using WaypointSystem;

namespace Managers
{
    public class DamageTextManager : Singleton<DamageTextManager>
    {
        public ObjectPooler Pooler { get; private set; }

        private void Start()
        {
            Pooler = GetComponent<ObjectPooler>();
        }
    }
}
