using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public class CompaniesController : MonoBehaviour, IDisposable
    {
        private IProvider<MissionInfo> companiesProvider;
        private Dictionary<int, MissionInfo> missionInfos;

        public void Initialize(IProvider<MissionInfo> provider)
        {
            companiesProvider = provider;
            missionInfos = provider.LoadAll()
                .ToDictionary(m => m.MissionId, m => m);
        }

        public void DefeatMission(int missionId)
        {
            missionInfos[missionId].MissionStatus = MissionStatus.Failed;
        }
        
        public void WinMission(int missionId)
        {
            missionInfos[missionId].MissionStatus = MissionStatus.Complete;
        }
        
        public void TryAddNewMission(int missionId)
        {
            missionInfos.TryAdd(missionId, new MissionInfo()
            {
                MissionId = missionId,
                MissionStatus = MissionStatus.NotVisited,
                MissionProgress = 0f
            });
        }

        public MissionInfo GetMissionInfo(int missionId)
        {
            return missionInfos[missionId];
        }
        
        
        public void Dispose()
        {
            
        }
    }
}