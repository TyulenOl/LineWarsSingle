using System;
using UnityEngine;
using UnityEngine.Audio;

namespace LineWars.Controllers
{
    public enum VolumeType
    {
        Master,
        Music,
        SFX
    }
    public class VolumeUpdater : MonoBehaviour
    {
        public static VolumeUpdater Instance { get; private set; }
        [SerializeField] private AudioMixer mixer;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("More than two VolumeUpdaters on the Scene");
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            LoadSettings();
        }

        public void OnDisable()
        {
            Save();
        }

        private void LoadSettings()
        {
            foreach (VolumeType channelType in Enum.GetValues(typeof(VolumeType)))
            {
                LoadChannel(channelType);
            }
            
            void LoadChannel(VolumeType channelType)
            {
                var param = channelType.ToString();
                if (!PlayerPrefs.HasKey(param))
                {
                    PlayerPrefs.SetFloat(param, 1);
                }

                var volume = FloatToMixer(PlayerPrefs.GetFloat(param));
                mixer.SetFloat(param, volume);
            }
        }
        
        public void Save()
        {
            foreach (VolumeType volumeType in Enum.GetValues(typeof(VolumeType)))
            {
                SaveChannel(volumeType);
            }
            
            void SaveChannel(VolumeType channelType)
            {
                var param = channelType.ToString();
                mixer.GetFloat(param, out var mixerVolume);
                var volume = MixerToFloat(mixerVolume);
                PlayerPrefs.SetFloat(param, volume);
            }
            
            PlayerPrefs.Save();
        }
        
        public void SetVolume(VolumeType channel, float value)
        {
            var mixerParam = channel.ToString();
            if (value == 0)
            {
                mixer.SetFloat(mixerParam, -80);
                return;
            }
            mixer.SetFloat(mixerParam, FloatToMixer(value));
        }

        public float GetVolume(VolumeType channel)
        {
            var mixerParam = channel.ToString();
            mixer.GetFloat(mixerParam, out var value);
            return MixerToFloat(value);
        }

        public static float MixerToFloat(float value)
        {
            return Mathf.Pow(10, value / 20);
        }

        public static float FloatToMixer(float value)
        {
            return Mathf.Log10(value) * 20;
        }
    }
}

