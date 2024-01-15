using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public class CompaniesController : MonoBehaviour
    {
        [SerializeField] private List<int> defaultUnlockMissionIds;
        
        [Header("Debug")]
        [SerializeField] private bool godMode;
        
        private IProvider<MissionInfo> companiesProvider;
        private IStorage<MissionData> missionStorage;
        
        private Dictionary<int, MissionInfo> missionInfos;

        private readonly HashSet<MissionInfo> missionsToSave = new ();
        
        public int ChoseMissionId { get; set; }
        public event Action AnyMissionInfoChanged;

        public void Initialize(IProvider<MissionInfo> provider, IStorage<MissionData> storage)
        {
            companiesProvider = provider;
            missionStorage = storage;
            
            missionInfos = provider.LoadAll()
                .ToDictionary(m => m.MissionId, m => m);

            foreach (var storageKey in missionStorage.Keys)
                TryAddNewMission(storageKey);

            // открытие изначальных миссий
            foreach (var id in defaultUnlockMissionIds)
            {
                if (missionInfos.ContainsKey(id))
                    UnlockMission(id);
            }
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
            missionInfo.MissionStatus = MissionStatus.Completed;
            missionsToSave.Add(missionInfo);
        }
        
        private void TryAddNewMission(int missionId)
        {
            missionInfos.TryAdd(missionId, new MissionInfo()
            {
                MissionId = missionId,
                MissionStatus = MissionStatus.Locked
            });
        }

        public MissionInfo GetMissionInfo(int missionId)
        {
            return missionInfos[missionId].GetCopy();
        }

        public void UnlockMission(int missionId)
        {
            if (missionInfos[missionId].MissionStatus == MissionStatus.Locked)
            {
                missionInfos[missionId].MissionStatus = MissionStatus.Unlocked;
                missionsToSave.Add(missionInfos[missionId]);
            }
        }
        
        public void UnlockNextMission()
        {
            if (missionInfos.ContainsKey(ChoseMissionId + 1))
                UnlockMission(ChoseMissionId + 1);
        }

        public bool MissionIsLocked(int missionId)
        {
            return !godMode || missionInfos[missionId].MissionStatus == MissionStatus.Locked;
        }

        public bool CompanyIsLocked(IEnumerable<int> companyMissions)
        {
            return companyMissions.All(MissionIsLocked);
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

        private void OnValidate()
        {
            if (Application.isPlaying)
                AnyMissionInfoChanged?.Invoke();
        }
    }
}