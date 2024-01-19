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

        public bool IsInfinityGameUnlocked => godMode || missionInfos[0].MissionStatus == MissionStatus.Completed;

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
            
            AssignAllMissions();
        }

        private void AssignAllMissions()
        {
            foreach (var (id, missionInfo) in missionInfos)
            {
                if (missionInfo.MissionStatus == MissionStatus.Completed
                    && missionInfos.ContainsKey(id + 1)
                    && missionInfos[id + 1].MissionStatus == MissionStatus.Locked)
                {
                    missionInfos[id + 1].MissionStatus = MissionStatus.Unlocked;
                }
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
            var copy = missionInfos[missionId].GetCopy();
            if (godMode && copy.MissionStatus == MissionStatus.Locked)
                copy.MissionStatus = MissionStatus.Unlocked;
            return copy;
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

        public MissionStatus GetMissionStatus(int missionId)
        {
            var mission = missionInfos[missionId];
            if (godMode && mission.MissionStatus == MissionStatus.Locked)
                return MissionStatus.Unlocked;
            return mission.MissionStatus;

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