using UnityEngine;
using UnityEngine.Serialization;

namespace LineWars
{
    /// <summary>
    /// Вся информация о миссии, включая изменения игрока
    /// </summary>
    [System.Serializable]
    public class MissionState
    {
        public MissionData missionData;
        public bool isCompleted;

        public MissionState(MissionData missionData, bool isCompleted)
        {
            this.missionData = missionData;
            this.isCompleted = isCompleted;
        }
    }
}