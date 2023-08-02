using System.Linq;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class CompanyElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text companyName;
        [SerializeField] private TMP_Text companyDescription;
        [SerializeField] private Image companyImage;
        [SerializeField] private TMP_Text missionsProgress;
        [SerializeField] private TMP_Text companyNation;

        public void Initialize(CompanyState companyState)
        {
            if (companyState == null) return;
            
            var data = companyState.CompanyData;

            companyName.text = data.Name;
            companyDescription.text = data.Description;
            companyImage.sprite = data.Image;

            var finishMissionsCount = companyState.MissionStates
                .Count(x => x.IsCompleted);
            
            var allMissionCount = companyState.MissionStates.Count;
            missionsProgress.text = $"{finishMissionsCount}/{allMissionCount}";
            companyNation.text = NationHelper.GetNationName(data.Nation);
        }
    }
}