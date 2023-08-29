using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace LineWars
{
    /// <summary>
    /// Вся информация о компании, включая изменения игрока
    /// </summary>
    [System.Serializable]
    public class CompanyState
    {
        public CompanyData companyData;
        public List<MissionState> missionStates;

        public CompanyState(CompanyData companyData)
        {
            this.companyData = companyData;
            missionStates = companyData.Missions
                .Select(
                    missionData => new MissionState
                    (
                        missionData,
                        false
                    )
                ).ToList();
        }
    }
}