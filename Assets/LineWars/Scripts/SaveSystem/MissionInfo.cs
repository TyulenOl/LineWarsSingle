using System;

namespace LineWars
{
    [Serializable]
    public class MissionInfo
    {
        public int MissionId;
        public MissionStatus MissionStatus;
        public float MissionProgress;
    }

    public enum MissionStatus
    {
        Failed,
        Complete,
        NotVisited
    }
}