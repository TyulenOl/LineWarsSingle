using System;
using UnityEngine;

namespace LineWars.Controllers.Speech
{
    [RequireComponent(typeof(AudioSource))]
    public class SpeechManager : MonoBehaviour
    {
        public static SpeechManager Instance { get; private set; }
        private AudioSource source;
        public event Action SpeechEnded;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            source = GetComponent<AudioSource>();
        }

        public void Play(AudioClip audioClip)
        {
            if (audioClip == null)
                return;
            source.PlayOneShot(audioClip);
            Invoke(nameof(InvokeSpeechEnded), audioClip.length);
        }

        private void InvokeSpeechEnded()
        {
            SpeechEnded?.Invoke();
        }
    }
}