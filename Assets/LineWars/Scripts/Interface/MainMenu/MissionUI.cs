using System;
using LineWars.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LineWars
{
    public class MissionUI : MonoBehaviour
    {
        [SerializeField] private bool automaticRedraw = true;
        [SerializeField] private MissionData missionData;
        
        [Header("References")]
        [SerializeField] private TMP_Text missionName;
        [SerializeField] private Image completedImage;
        [SerializeField] private Image uncompletedImage;
        [SerializeField] private Button button;
        
        
        private MissionInfoUI missionInfoUI;
        private bool initialized;
        private int id;
        
        public MissionInfo MissionInfo { get; private set; }

        public void Initialize(MissionInfoUI infoUI)
        {
            if (initialized)
            {
                Debug.LogError($"{nameof(MissionUI)} is initialized!");
                return;
            }
            initialized = true;
            
            if (missionData == null)
                Debug.LogError($"{nameof(missionData)} is null on {name}", gameObject);
            id = GameRoot.Instance.MissionsStorage.ValueToId[missionData];
            
            missionInfoUI = infoUI;
            
            button.onClick.AddListener(OnClick);
            
            MissionInfo = GameRoot.Instance.CompaniesController.GetMissionInfo(id);
            
            completedImage.gameObject.SetActive(MissionInfo.MissionStatus == MissionStatus.Completed);
            uncompletedImage.gameObject.SetActive(MissionInfo.MissionStatus != MissionStatus.Completed);
        }

        private void OnClick()
        {
            GameRoot.Instance.CompaniesController.ChoseMissionId = id;
            missionInfoUI.Redraw(missionData);
        }

        private void RedrawMissionData(MissionData data)
        {
            missionName.text = data.MissionName;
        }

        private void OnValidate()
        {
            if (automaticRedraw && missionData != null)
            {
                RedrawMissionData(missionData);
            }
        }
    }
}