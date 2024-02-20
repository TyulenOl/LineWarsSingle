using System;
using LineWars.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class SendMetricaButton: MonoBehaviour
    {
        [SerializeField] private string buttonId;
        private Button button;
        private static SDKAdapterBase SDKAdapter => GameRoot.Instance?.SdkAdapter;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            if (button != null)
                button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            if (SDKAdapter != null && !string.IsNullOrEmpty(buttonId))
            {
                SDKAdapter.SendButtonMetrica(buttonId);
            }
        }
    }
}