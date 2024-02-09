using System;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    [RequireComponent(typeof(Button))]
    public class ResumeButton: MonoBehaviour
    {
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            button.onClick.AddListener(Resume);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(Resume);
        }

        private void Resume()
        {
            PauseInstaller.Pause(false);
        }
    }
}