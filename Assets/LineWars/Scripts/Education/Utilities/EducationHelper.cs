using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Education
{
    public class EducationHelper: MonoBehaviour
    {
        public void WinEducation()
        {
            if(GameRoot.Instance != null)
            {
                GameRoot.Instance.CompaniesController.WinChoseMission();
                GameRoot.Instance.CompaniesController.UnlockNextMission();
            }
        }
    }
}