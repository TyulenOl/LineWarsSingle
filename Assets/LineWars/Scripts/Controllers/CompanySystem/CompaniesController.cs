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
        [SerializeField] private bool debugMode;
        
        private IProvider<MissionInfo> companiesProvider;
        private IStorage<MissionData> missionStorage;
        
        private Dictionary<int, MissionInfo> missionInfos;
        
        public int ChoseMissionId { get; set; }
        public event Action AnyMissionInfoChanged;

        public bool IsInfinityGameUnlocked => debugMode || missionInfos[0].MissionStatus == MissionStatus.Completed;

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

        public void DefeatChoseMissionIfNoWin()
        {
            var missionInfo = missionInfos[ChoseMissionId];
            if (missionInfo.MissionStatus 
                is MissionStatus.Completed 
                or MissionStatus.Failed)
                return;
            
            missionInfo.MissionStatus = MissionStatus.Failed;
            SaveMission(missionInfo);
        }
        
        public void WinChoseMission()
        {
            var missionInfo = missionInfos[ChoseMissionId];
            if (missionInfo.MissionStatus is MissionStatus.Completed) return;
            
            missionInfo.MissionStatus = MissionStatus.Completed;
            SaveMission(missionInfo);
        }
        
        public void UnlockMission(int missionId)
        {
            var missionInfo = missionInfos[missionId];
            if (missionInfo.MissionStatus is not MissionStatus.Locked) return;
            
            
            missionInfo.MissionStatus = MissionStatus.Unlocked;
            SaveMission(missionInfo);
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
            if (debugMode && copy.MissionStatus == MissionStatus.Locked)
                copy.MissionStatus = MissionStatus.Unlocked;
            return copy;
        }
        
        public void UnlockNextMission()
        {
            if (missionInfos.ContainsKey(ChoseMissionId + 1))
                UnlockMission(ChoseMissionId + 1);
        }

        public MissionStatus GetMissionStatus(int missionId)
        {
            var mission = missionInfos[missionId];
            if (debugMode && mission.MissionStatus == MissionStatus.Locked)
                return MissionStatus.Unlocked;
            return mission.MissionStatus;

        }

        private void SaveMission(MissionInfo info)
        {
            if (!debugMode) 
                companiesProvider.Save(info, info.MissionId);
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
                AnyMissionInfoChanged?.Invoke();
        }
    }
}