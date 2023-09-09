using System;
using System.Collections;
using System.Collections.Generic;
using LineWars;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class VolumeUpdater : MonoBehaviour
{
    public static VolumeUpdater Instance { get; private set; }
    [SerializeField] private string musicMixerParam;
    [SerializeField] private string sfxMixerParam;
    [field: SerializeField] public AudioMixer Mixer { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("More than two VolumeUpdaters on the Scene");
        }
    }

    private void OnEnable()
    {
        Debug.Log(PlayerPrefs.GetFloat(musicMixerParam));
        if (!PlayerPrefs.HasKey(musicMixerParam))
        {
            PlayerPrefs.SetFloat(musicMixerParam, 1);
        }

        if (!PlayerPrefs.HasKey(sfxMixerParam))
        {
            PlayerPrefs.SetFloat(sfxMixerParam, 1);
        }
        
        var musicVolume = Mathf.Log10(PlayerPrefs.GetFloat(musicMixerParam)) * 20;
        var sfxVolume = Mathf.Log10(PlayerPrefs.GetFloat(sfxMixerParam)) * 20;
        Mixer.SetFloat(sfxMixerParam, musicVolume);
        Mixer.SetFloat(musicMixerParam, sfxVolume);
    }

    public void SetVolume(string mixerParam, float value)
    {
        if (value == 0)
        {
            Mixer.SetFloat(mixerParam, -80);
            return;
        }
        Mixer.SetFloat(mixerParam, Mathf.Log10(value) * 20);
    }

    public void OnDisable()
    {
        Mixer.GetFloat(musicMixerParam, out var musicMixerVolume);
        var musicVolume = Mathf.Pow(10, musicMixerVolume / 20);
        Mixer.GetFloat(sfxMixerParam, out var sfxMixerVolume);
        var sfxVolume = Mathf.Pow(10, sfxMixerVolume / 20);
        PlayerPrefs.SetFloat(musicMixerParam, musicVolume);
        PlayerPrefs.SetFloat(sfxMixerParam, sfxVolume);
        PlayerPrefs.Save();
        Debug.Log("Disable");
    }
}
