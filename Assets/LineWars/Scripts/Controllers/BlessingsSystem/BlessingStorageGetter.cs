using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public class BlessingStorageGetter: MonoBehaviour, IGetter<IStorage<BlessingId, BaseBlessing>>
    {
        public bool CanGet()
        {
            return GameRoot.Instance != null;
        }

        public IStorage<BlessingId, BaseBlessing> Get()
        {
            if (GameRoot.Instance != null)
                return GameRoot.Instance.BlessingStorage;
            return ScriptableObject.CreateInstance<BlessingStorage>();
        }
    }
}