using System;
using System.Collections;
using UnityEngine;

namespace LineWars.Controllers
{
    [RequireComponent(typeof(AudioSource))]
    public class SpeechManager : MonoBehaviour
    {
        public static SpeechManager Instance { get; private set; }
        private AudioSource source;
        public AudioSource Source => source;

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
            StartCoroutine(PlayCoroutine(audioClip));
        }

        private IEnumerator PlayCoroutine(AudioClip audioClip)
        {
            source.PlayOneShot(audioClip);
            while (source.isPlaying)
                yield return null;
            InvokeSpeechEnded();
        }
        
        public void StopAllSounds()
        {
            StopAllCoroutines();
            source.Stop();
        }
        
        private void InvokeSpeechEnded()
        {
            SpeechEnded?.Invoke();
        }
    }
}