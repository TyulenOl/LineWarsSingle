using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars
{
    [CreateAssetMenu(fileName = "New Music Data", menuName = "Audio/Plugin/Music Logic/Music Data")]
    public class PluginMusicData : ScriptableObject
    {
        [SerializeField] private string audioClipKey;
        [SerializeField] private string songName;
        [SerializeField] private string artist;
        [SerializeField, Range(0, 1)] private float volumeCoeficient;
        [SerializeField] private bool isAuthorMusic;

        public string AudioClipKey => audioClipKey;
        public string Name => songName;
        public string Artist => artist;
        public float VolumeCoeficient => volumeCoeficient;
        public bool IsAuthorMusic => isAuthorMusic;
    }
}
