using System;

namespace LineWars
{
    [Serializable]
    public class MissionInfo
    {
        public int MissionId;
        public MissionStatus MissionStatus;

        public MissionInfo GetCopy()
        {
            return new MissionInfo()
            {
                MissionId = MissionId,
                MissionStatus = MissionStatus,
            };
        }
    }

    public enum MissionStatus
    {
        Completed,
        Unlocked,
        Locked,
        Failed
    }
}