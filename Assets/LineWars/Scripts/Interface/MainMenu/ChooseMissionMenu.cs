﻿using System;
using UnityEngine;

namespace LineWars
{
    public class ChooseMissionMenu: UIStackElement
    {
        public static ChooseMissionMenu Instance { get; private set; }
        
        [SerializeField] private MissionInfoUI missionInfoUI;
        [SerializeField] private Transform missions;

        private MissionsMapUI initializedMap;

        protected override void Awake()
        {
            base.Awake();
            if (Instance == null)
                Instance = this;
            else
            {
                Debug.LogError($"Dublicated {nameof(ChooseMissionMenu)}");
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            CheckValid();
        }
        
        private void CheckValid()
        {
            if (missionInfoUI == null)
                Debug.LogError($"{nameof(missionInfoUI)} is null on {name}");
            
            if (missions == null)
                Debug.LogError($"{nameof(missions)} is null on {name}");
        }

        public void Initialize(CompanyState companyState)
        {
            var data = companyState.CompanyData;
            if (initializedMap != null)
                Destroy(initializedMap.gameObject);
            var prefab = data.MissionsMapUIPrefab;
            initializedMap = Instantiate(prefab, missions);

            for (var i = 0; i < initializedMap.MissionUIs.Count; i++)
            {
                var missionUi = initializedMap.MissionUIs[i];
                var missionState = companyState.MissionStates[i]; 
                missionUi.Initialize(missionState, i, OnMissionUiClick);
            }
        }

        private void OnMissionUiClick(MissionState state)
        {
            missionInfoUI.gameObject.SetActive(true);
            missionInfoUI.Initialize(state);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            missionInfoUI.gameObject.SetActive(false);
        }
    }
}