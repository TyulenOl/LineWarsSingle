using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public class CompaniesController : MonoBehaviour
    {
        private IProvider<MissionInfo> companiesProvider;
        private IStorage<MissionData> missionStorage;
        
        private Dictionary<int, MissionInfo> missionInfos;

        private readonly HashSet<MissionInfo> missionsToSave = new ();
        
        public int ChoseMissionId { get; set; }

        public void Initialize(IProvider<MissionInfo> provider, IStorage<MissionData> storage)
        {
            companiesProvider = provider;
            missionStorage = storage;
            
            missionInfos = provider.LoadAll()
                .ToDictionary(m => m.MissionId, m => m);

            foreach (var storageKey in missionStorage.Keys)
                TryAddNewMission(storageKey);
        }

        public void DefeatChoseMission()
        {
            var missionInfo = missionInfos[ChoseMissionId];
            missionInfo.MissionStatus = MissionStatus.Failed;
            missionsToSave.Add(missionInfo);
        }
        
        public void WinChoseMission()
        {
            var missionInfo = missionInfos[ChoseMissionId];
            missionInfo.MissionStatus = MissionStatus.Complete;
            missionsToSave.Add(missionInfo);
        }
        
        private void TryAddNewMission(int missionId)
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
        
        
        public void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                foreach (var missionInfo in missionsToSave)
                {
                    companiesProvider.Save(missionInfo, missionInfo.MissionId);
                }
                missionsToSave.Clear();
            }
        }
    }
}