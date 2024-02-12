﻿using System;
using System.Linq;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class CompanyElementUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text companyName;
        [SerializeField] private TMP_Text companyDescription;
        [SerializeField] private Image companyImage;
        [SerializeField] private TMP_Text missionsProgress;
        [SerializeField] private Button companyElementButton;
        [SerializeField] private Image bg;

        [SerializeField] private CompanyMenu menuToOpen;

        [SerializeField] private bool automaticRedraw = true;
        [SerializeField] private CompanyData companyData;
        
        private bool initialized;

        public void Initialize()
        {
            if (initialized)
            {
                Debug.LogError($"{GetType().Name} is initialized!");
                return;
            }
            initialized = true;
            
            if (menuToOpen != null)
            {
                menuToOpen.Initialize();
                var missionsInfos = menuToOpen.MissionInfos.ToList();
                var completedMissionsCount = missionsInfos.Count(x => x.MissionStatus == MissionStatus.Completed);
                missionsProgress.text = $"{completedMissionsCount}/{missionsInfos.Count}";
                companyElementButton.onClick.AddListener(OnClick);
                var isOpen = missionsInfos.Any(x => x.MissionStatus != MissionStatus.Locked);
                bg.gameObject.SetActive(!isOpen);
                companyElementButton.interactable = isOpen;
            }
            else
            {
                bg.gameObject.SetActive(true);
                companyElementButton.interactable = false;
            }
        }

        private void OnClick()
        {
            UIStack.Instance.PushElement(menuToOpen.transform);
        }

        private void RedrawCompanyData(CompanyData data)
        {
            companyName.text = data.Name;
            companyDescription.text = data.Description;
            companyImage.sprite = data.Image;
        }

        private void OnValidate()
        {
            if (automaticRedraw && companyData != null)
                RedrawCompanyData(companyData);
        }
    }
}