using System;
using LineWars.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    [RequireComponent(typeof(Toggle))]
    public class EnableAuthorMusicToggle: MonoBehaviour
    {
        private Toggle toggle;

        private void Awake()
        {
            toggle = GetComponent<Toggle>();
        }

        private void OnEnable()
        {
            if (MusicSettings.Instance)
                toggle.isOn = MusicSettings.Instance.EnableAuthorMusic; 
            
            toggle.onValueChanged.AddListener(ToggleValueChanged);
        }

        private void OnDisable()
        {
            toggle.onValueChanged.RemoveListener(ToggleValueChanged);
        }

        private void ToggleValueChanged(bool value)
        {
            if (MusicSettings.Instance)
                MusicSettings.Instance.EnableAuthorMusic = value;
        }
    }
}