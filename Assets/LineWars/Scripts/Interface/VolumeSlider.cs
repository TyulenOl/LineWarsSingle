using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private string mixerParam;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        VolumeUpdater.Instance.Mixer.GetFloat(mixerParam, out var mixerVolume);
        var volume = Mathf.Pow(10, mixerVolume / 20);
        slider.value = volume;
        slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        VolumeUpdater.Instance.SetVolume(mixerParam, value);
    }
}
