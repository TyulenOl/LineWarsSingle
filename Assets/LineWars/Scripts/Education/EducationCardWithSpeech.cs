using System;
using LineWars.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Education
{
    public class EducationCardWithSpeech : EducationCardBase
    {
        [SerializeField] private AudioClip speech;
        private Button button;

        private void Awake()
        {
            button = GetComponentInChildren<Button>();
            if(button != null)
                button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            SpeechManager.Instance.StopAllSounds();
            carousel.Next();
        }

        private void OnEnable()
        {
            CommandsManager.Instance.Deactivate();
            SpeechManager.Instance.SpeechEnded += ManagerOnSpeechEnded;
            if (!SpeechManager.Instance.Source.isPlaying)
                SpeechManager.Instance.Play(speech);
        }

        private void ManagerOnSpeechEnded()
        {
            carousel.Next();
        }

        private void OnDisable()
        {
            SpeechManager.Instance.SpeechEnded -= ManagerOnSpeechEnded;
        }
    }
}