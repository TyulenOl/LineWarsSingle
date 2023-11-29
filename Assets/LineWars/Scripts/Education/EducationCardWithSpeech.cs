using System;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Education
{
    public class EducationCardWithSpeech : EducationCardBase
    {
        [Header("")] [SerializeField] private SpeechManager manager;
        [SerializeField] private AudioClip speech;

        private void OnEnable()
        {
            CommandsManager.Instance.Deactivate();
            manager.SpeechEnded += ManagerOnSpeechEnded;
            manager.Play(speech);
        }

        private void ManagerOnSpeechEnded()
        {
            carousel.Next();
        }

        private void OnDisable()
        {
            manager.SpeechEnded -= ManagerOnSpeechEnded;
        }
    }
}