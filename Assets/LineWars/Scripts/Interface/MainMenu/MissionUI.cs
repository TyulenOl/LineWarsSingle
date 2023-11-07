using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LineWars
{
    public class MissionUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text missionName;
        [SerializeField] private Button completedButton;
        [SerializeField] private Button uncompletedButton;

        private MissionState currentState;
        private Action<MissionState> clicked;

        private void Awake()
        {
            CheckValid();
        }

        private void Start()
        {
            completedButton.onClick.AddListener(OnClickButton);
            uncompletedButton.onClick.AddListener(OnClickButton);
        }

        private void OnClickButton()
        {
            clicked?.Invoke(currentState);
        }

        private void CheckValid()
        {
            if (missionName == null)
                Debug.LogError($"{nameof(missionName)} is null on {name}");
        }

        public void Initialize(MissionState state, Action<MissionState> clicked)
        {
            var data = state.missionData;
            missionName.text = $"{data.MissionName}";
            this.clicked = clicked;
            this.currentState = state;
            completedButton.gameObject.SetActive(state.isCompleted);
            uncompletedButton.gameObject.SetActive(!state.isCompleted);
        }
    }
}