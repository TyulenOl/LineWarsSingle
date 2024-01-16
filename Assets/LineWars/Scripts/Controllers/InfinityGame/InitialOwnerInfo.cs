using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(menuName = "InfinityGame/InitialOwnerInfo", order = 59)]
    public class InitialOwnerInfo: ScriptableObject
    {
        [field: SerializeField] public List<InitialNodeInfo> NodeInfos { get; set; }
    }
}