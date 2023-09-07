using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace LineWars.Controllers
{
    [RequireComponent(typeof(AudioSource))] 
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance { get; private set; }
        [SerializeField] private MusicLogicData musicLogicData;
        
        [Header("Fade Out Settings")]
        [SerializeField] private AnimationCurve fadeOutCurve;
        [SerializeField] private float fadeOutTime;

        [Header("Fade In Settings")]
        [SerializeField] private AnimationCurve fadeInCurve;
        [SerializeField] private float fadeInTime;

        private AudioSource source;
        private MusicLogic logic;

        public AudioSource Source => source;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Instance.SwitchMusicLogic(musicLogicData);
                Destroy(gameObject);
                return;
            }

            source = GetComponent<AudioSource>();
            logic = musicLogicData.GetMusicLogic(this);
        }

        private void Start()
        {
            logic.Start();
        }

        private void Update()
        {
            logic.Update();
        }

        private void SwitchMusicLogic(MusicLogicData data)
        {
            StartCoroutine(SwitchCoroutine());
            IEnumerator SwitchCoroutine()
            {
                var passedTime = 0f;
                while (passedTime < fadeOutTime)
                {
                    yield return null;
                    passedTime += Time.deltaTime;
                    source.volume = fadeOutCurve.Evaluate(passedTime / fadeOutTime);
                }
                logic.Exit();
                logic = data.GetMusicLogic(this);
                logic.Start();
                
                passedTime = 0f;
                while (passedTime < fadeInTime)
                {
                    yield return null;
                    passedTime += Time.deltaTime;
                    source.volume = fadeInCurve.Evaluate(passedTime / fadeOutTime);
                }
            }
        }
    }
}

