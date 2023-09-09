using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LineWars
{
    [RequireComponent(typeof(Button))]
    public class MissionUI: MonoBehaviour
    {
        [SerializeField] private TMP_Text missionName;
        private Button button;

        private MissionState currentState;
        private Action<MissionState> clicked;

        private void Awake()
        {
            CheckValid();
        }

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClickButton);
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
        
        public void Initialize(MissionState state, int index, Action<MissionState> clicked)
        {
            var data = state.missionData;
            missionName.text = $"{index} - {data.MissionName}";
            this.clicked = clicked;
            this.currentState = state;
        }
    }
}