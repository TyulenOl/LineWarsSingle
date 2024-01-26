using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(menuName = "Storages/MissionsStorage", order = 53)]
    public class MissionsScriptableStorage: ScriptableStorage<int, MissionData>
    {
        
    }
}