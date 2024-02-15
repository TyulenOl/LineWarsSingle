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
        [SerializeField, Range(0, 1)] private float volumeCoeficient;
        [SerializeField] private bool isAuthorMusic;

        public AudioClip AudioClip => audioClip;
        public string Name => songName;
        public string Artist => artist;
        public float VolumeCoeficient => volumeCoeficient;
        public bool IsAuthorMusic => isAuthorMusic;
    }
}
