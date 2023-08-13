using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars
{
    /// <summary>
    /// Вся информация о компании, включая изменения игрока
    /// </summary>
    [System.Serializable]
    public class CompanyState
    {
        [SerializeField] private CompanyData companyData;
        [SerializeField] private List<MissionState> missionStates;

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

        public CompanyState(CompanyData companyData, IEnumerable<MissionState> missionStates)
        {
            this.companyData = companyData;
            this.missionStates = missionStates.ToList();
        }

        public CompanyData CompanyData => companyData;
        public List<MissionState> MissionStates => missionStates;
    }
}