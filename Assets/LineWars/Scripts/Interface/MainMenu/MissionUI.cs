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
        [SerializeField] private Image missionImage;
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

            ReDrawMissionImageByStatus();
        }

        private void ReDrawMissionImageByStatus()
        {
            var status = GameRoot.Instance.CompaniesController.GetMissionStatus(MissionInfo.MissionId);
            missionImage.sprite = DrawHelper.GetSpriteByMissionStatus(status);
            button.interactable = status != MissionStatus.Locked;
        }
        
        private void OnClick()
        {
            GameRoot.Instance.CompaniesController.ChoseMissionId = id;
            missionInfoUI.Redraw(missionData, DrawHelper.GetOnMissionButtonTextByMissionStatus(GameRoot.Instance.CompaniesController.GetMissionStatus(MissionInfo.MissionId)));
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