using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LineWars
{
    public class MissionUI: MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TMP_Text missionName;

        private MissionState currentState;
        private Action<MissionState> clicked;

        private void Awake()
        {
            CheckValid();
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

        public void OnPointerClick(PointerEventData eventData)
        {
            clicked?.Invoke(currentState);
        }
    }
}