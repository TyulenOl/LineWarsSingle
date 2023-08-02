using UnityEngine;

namespace LineWars
{
    /// <summary>
    /// Вся информация о миссии, включая изменения игрока
    /// </summary>
    [System.Serializable]
    public class MissionState
    {
        [SerializeField] private MissionData missionData;
        [SerializeField] private bool isCompleted;

        public MissionState(MissionData missionData, bool isCompleted)
        {
            this.missionData = missionData;
            this.isCompleted = isCompleted;
        }

        public MissionData MissionData => missionData;
        public bool IsCompleted => isCompleted;
    }
}