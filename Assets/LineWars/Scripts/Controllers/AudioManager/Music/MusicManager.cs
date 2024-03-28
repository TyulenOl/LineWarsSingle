using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Controllers
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance { get; private set; }
        [SerializeField] private MusicLogicData musicLogicData;
        [SerializeField] private TwoStringEventChannel raisedMusicChangedEventChannel;
        
        [Header("Fade Out Settings")]
        [SerializeField] private AnimationCurve fadeOutCurve;
        [SerializeField] private float fadeOutTime;

        [Header("Fade In Settings")]
        [SerializeField] private AnimationCurve fadeInCurve;
        [SerializeField] private float fadeInTime;

        private AudioSource source;
        private MusicLogic logic;
        private float initialVolume;
        public MusicData CurrentMusicData { get; private set; }
        public UnityEvent<MusicData, MusicData> MusicChanged;
        public bool IsPlaying => source.isPlaying;

        //  public AudioSource Source => source;
        public float Volume
        {
            get => source.volume;
            set => source.volume = value;
        }

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
            initialVolume = source.volume;
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

        public void Play(MusicData musicData)
        {
            var prevData = CurrentMusicData;
            source.clip = musicData.AudioClip;
            source.volume = source.volume * musicData.VolumeCoeficient;
            source.Play();
            CurrentMusicData = musicData;
            MusicChanged.Invoke(prevData, musicData);
            raisedMusicChangedEventChannel.RaiseEvent(musicData.Name, musicData.Artist);
        }

        public void Stop()
        {
            if(CurrentMusicData == null)
                return;
            var prevData = CurrentMusicData;
            CurrentMusicData = null;
            source.Stop();
            source.clip = null;
            MusicChanged.Invoke(prevData, null);
        }
    }
}

