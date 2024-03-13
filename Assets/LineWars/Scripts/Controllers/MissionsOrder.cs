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
            var index = order.FindIndex(missionData);
            if (index < 0 || index >= order.Count)
                return null;
            return order[index + 1];
        }
    }
}