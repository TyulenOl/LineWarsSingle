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
            manager.Play(speech);
            manager.SpeechEnded += ManagerOnSpeechEnded;
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