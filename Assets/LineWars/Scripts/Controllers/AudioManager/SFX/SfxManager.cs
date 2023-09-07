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
            source.PlayOneShot(data.Clip);
        }

        public void StopAllSounds()
        {
            source.Stop();
        }
    }
}

