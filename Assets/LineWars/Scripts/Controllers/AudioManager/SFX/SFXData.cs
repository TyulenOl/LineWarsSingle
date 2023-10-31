using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(fileName = "New SFX Data", menuName = "Audio/SFX")]
    public class SFXData : ScriptableObject
    {
        [SerializeField] private AudioClip clip;
        public float LengthInSeconds => clip.length;
        public AudioClip Clip => clip;
    }
}

