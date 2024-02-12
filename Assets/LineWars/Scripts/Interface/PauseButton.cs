using System;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    [RequireComponent(typeof(Button))]
    public class PauseButton: MonoBehaviour
    {
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            button.onClick.AddListener(Pause);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(Pause);
        }

        private void Pause()
        {
            PauseInstaller.Pause(true);
        }
    }
}