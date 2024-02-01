using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "Storages/BlessingStorage")]
    public class BlessingStorage: ListScriptableStorage<BlessingId, BaseBlessing>
    {
        protected override BlessingId GetKey(BaseBlessing value)
        {
            return value.BlessingId;
        }
    }
}