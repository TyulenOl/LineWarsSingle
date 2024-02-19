using System;
using UnityEngine;
using Utilities.Runtime;

namespace LineWars
{
    [CreateAssetMenu(fileName = "New MissionData", menuName = "Data/Create MissionData", order = 50)]
    public class MissionData : ScriptableObject
    {
        [SerializeField] private string missionName;
        [SerializeField] [TextArea(5, 10)] private string missionDescription;
        [SerializeField] private Sprite missionImage;
        //[SerializeField] private SceneName sceneToLoad;
        [SerializeField] private StableEnum<SceneName> sceneToLoadStable;
        
        public string MissionName => missionName;
        public string MissionDescription => missionDescription;
        public Sprite MissionImage => missionImage;
        public SceneName SceneToLoad => sceneToLoadStable;
    }
}