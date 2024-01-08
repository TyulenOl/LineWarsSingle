using System;
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
                var completedMissionsCount = missionsInfos.Count(x => x.MissionStatus == MissionStatus.Complete);
                missionsProgress.text = $"{completedMissionsCount}/{missionsInfos.Count}";
                companyElementButton.onClick.AddListener(OnClick);
            }
        }

        private void OnClick()
        {
            UIStack.Instance.PushElement(menuToOpen);
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