using System;
using UnityEngine;

namespace LineWars
{
    [CreateAssetMenu(fileName = "New MissionData", menuName = "Data/Create MissionData", order = 50)]
    public class MissionData : ScriptableObject, IEquatable<MissionData>
    {
        [SerializeField] private string missionName;
        [SerializeField] [TextArea(5, 10)] private string missionDescription;
        [SerializeField] private Sprite missionImage;
        [SerializeField] private SceneName sceneToLoad;

        public string MissionName => missionName;
        public string MissionDescription => missionDescription;
        public Sprite MissionImage => missionImage;
        public SceneName SceneToLoad => sceneToLoad;

        public bool Equals(MissionData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other)
                   && missionName == other.missionName
                   && missionDescription == other.missionDescription
                   && Equals(missionImage, other.missionImage)
                   && sceneToLoad == other.sceneToLoad;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MissionData) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                base.GetHashCode(),
                missionName,
                missionDescription,
                missionImage,
                (int) sceneToLoad);
        }
    }
}