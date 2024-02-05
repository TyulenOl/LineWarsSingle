using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars
{
    [CreateAssetMenu(fileName = "New Music Data", menuName = "Audio/Music Logic/Music Data")]
    public class MusicData : ScriptableObject
    {
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private string songName;
        [SerializeField] private string artist;

        public AudioClip AudioClip => audioClip;
        public string Name => songName;
        public string Artist => artist;
    }
}
