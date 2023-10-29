using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Controllers
{
    [RequireComponent(typeof(AudioSource))]
    public class SfxManager : MonoBehaviour
    {
        public static SfxManager Instance { get; private set; }
    
        private AudioSource source;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }

            source = GetComponent<AudioSource>();
        }

        public void Play(SFXData data)
        {
            if(data != null)
                source.PlayOneShot(data.Clip);
        }

        public void PlayWithDelay(SFXData data, int delayInSeconds)
        {
            StartCoroutine(WaitForSound(delayInSeconds, data));
        }
        
        private IEnumerator WaitForSound(int seconds, SFXData data)
        {
            yield return new WaitForSeconds(seconds);
            Play(data);
        }

        public void StopAllSounds()
        {
            source.Stop();
        }
    }
}

