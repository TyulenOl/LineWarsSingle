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
        [SerializeField] private MissionsOrder missionsOrder;
        
        [Header("Debug")]
        [SerializeField] private bool debugMode;
        
        private IProvider<MissionInfo> companiesProvider;
        private IStorage<int, MissionData> missionStorage;
        
        private Dictionary<int, MissionInfo> missionInfos;
        
        public int ChosenMissionId { get; set; }
        public MissionInfo ChosenMission => GetMissionInfo(ChosenMissionId); 

        public bool IsInfinityGameUnlocked => debugMode || missionInfos.Values
            .Any(x => x.MissionStatus == MissionStatus.Completed);

        public void Initialize(IProvider<MissionInfo> provider, IStorage<int, MissionData> storage)
        {
            companiesProvider = provider;
            missionStorage = storage;
            
            LoadMissionsSaves();
            AssignAllMissions();
            UnlockDefaultMissions();
        }

        private void LoadMissionsSaves()
        {
            missionInfos = companiesProvider.LoadAll()
                .ToDictionary(m => m.MissionId, m => m);
        }

        private void AssignAllMissions()
        {
            // добавление новых миссий
            foreach (var storageKey in missionStorage.Keys)
                TryAddNewMission(storageKey);
            // удаление устаревших миссий
            foreach (var missionKey in missionInfos.Keys.ToArray())
            {
                if (!missionStorage.ContainsKey(missionKey))
                    missionInfos.Remove(missionKey);
            }
            
            // восстановление связей миссий по ордеру
            foreach (var (id, missionInfo) in missionInfos)
            {
                if (missionInfo.MissionStatus != MissionStatus.Completed)
                    continue;
                
                var nextMission = missionsOrder.FindNextMission(missionStorage.IdToValue[id]);
                if (nextMission == null || !missionStorage.ContainsValue(nextMission))
                    continue;
                var nextMissionId = missionStorage.ValueToId[nextMission];
                UnlockMission(nextMissionId);
            }
        }
        
        private void TryAddNewMission(int missionId)
        {
            missionInfos.TryAdd(missionId, new MissionInfo()
            {
                MissionId = missionId,
                MissionStatus = MissionStatus.Locked
            });
        }
        
        private void UnlockDefaultMissions()
        {
            // открытие изначальных миссий
            foreach (var id in defaultUnlockMissionIds)
            {
                if (missionInfos.ContainsKey(id))
                    UnlockMission(id);
            }
        }

        public void DefeatChoseMissionIfNoWin()
        {
            if (!missionInfos.ContainsKey(ChosenMissionId))
            {
                Debug.LogError($"No contains mission by id = {ChosenMissionId}");
                return;
            }
            
            var missionInfo = missionInfos[ChosenMissionId];
            if (missionInfo.MissionStatus 
                is MissionStatus.Completed 
                or MissionStatus.Failed)
                return;
            
            missionInfo.MissionStatus = MissionStatus.Failed;
            SaveMission(missionInfo);
        }
        
        public void WinChoseMission()
        {
            if (!missionInfos.ContainsKey(ChosenMissionId))
            {
                Debug.LogError($"No contains mission by id = {ChosenMissionId}");
                return;
            }
            
            var missionInfo = missionInfos[ChosenMissionId];
            if (missionInfo.MissionStatus is MissionStatus.Completed) return;
            
            missionInfo.MissionStatus = MissionStatus.Completed;
            SaveMission(missionInfo);
        }
        
        public void UnlockMission(int missionId)
        {
            if (!missionInfos.ContainsKey(missionId))
            {
                Debug.LogError($"No contains mission by id = {missionId}");
                return;
            }
            
            var missionInfo = missionInfos[missionId];
            if (missionInfo.MissionStatus is not MissionStatus.Locked) return;
            
            
            missionInfo.MissionStatus = MissionStatus.Unlocked;
            SaveMission(missionInfo);
        }

        public MissionInfo GetMissionInfo(int missionId)
        {
            var copy = missionInfos[missionId].GetCopy();
            if (debugMode && copy.MissionStatus == MissionStatus.Locked)
                copy.MissionStatus = MissionStatus.Unlocked;
            return copy;
        }

        public bool ContainsNextMission(int missionId)
        {
            if (!missionStorage.ContainsKey(missionId))
                return false;
            var nextMission = missionsOrder.FindNextMission(missionStorage.IdToValue[missionId]);
            return nextMission != null && missionStorage.ContainsValue(nextMission);
        }
        
        public void UnlockNextMission()
        {
            if (!missionInfos.ContainsKey(ChosenMissionId))
            {
                Debug.LogError($"No contains mission by id = {ChosenMissionId}");
                return;
            }
            
            var nextMission = missionsOrder.FindNextMission(missionStorage.IdToValue[ChosenMissionId]);
            if (nextMission != null && missionStorage.ContainsValue(nextMission))
                UnlockMission(missionStorage.ValueToId[nextMission]);
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
    }
}