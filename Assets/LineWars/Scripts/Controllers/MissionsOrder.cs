using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(menuName = "Data/MissionOrder")]
    public class MissionsOrder: ScriptableObject
    {
        [SerializeField] private List<MissionData> order;

        public MissionData FindNextMission(MissionData missionData)
        {
            if (order == null || missionData == null)
                return null;
            
            var index = order.IndexOf(missionData);
            if (index < 0 || index + 1 >= order.Count)
                return null;
            return order[index];
            
        }
    }
}