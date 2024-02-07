using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;
using YG;

namespace LineWars
{
    public class YandexGameProvider: MonoBehaviour,
        IProvider<MissionInfo>,
        IProvider<DeckInfo>,
        IProvider<UserInfo>
    {
        private bool isLoaded;
        private GameInfo gameInfo;

        public bool IsLoaded => isLoaded;
        public event Action FinishLoad;

        private void OnEnable() => YandexGame.GetDataEvent += DownLoadAll;
        private void OnDisable() => YandexGame.GetDataEvent -= DownLoadAll;
        private void DownLoadAll()
        {
            isLoaded = true;
            gameInfo = new GameInfo().UpdateInfo(YandexGame.savesData);
            FinishLoad?.Invoke();
        }
        
        void ISaver<MissionInfo>.Save(MissionInfo value, int id)
        {
            gameInfo.Missions ??= new Dictionary<int, MissionInfo>();
            gameInfo.Missions[id] = value;
            UpdateYGSave();
        }
        
        MissionInfo IDownloader<MissionInfo>.Load(int id)
        {
            if (!isLoaded)
                throw new InvalidOperationException();
            if (gameInfo.Missions == null || !gameInfo.Missions.ContainsKey(id))
                return null;
            return gameInfo.Missions[id];
        }

        IEnumerable<MissionInfo> IAllDownloader<MissionInfo>.LoadAll()
        {
            if (!isLoaded)
                throw new InvalidOperationException();
            return gameInfo.Missions?.Values ?? Enumerable.Empty<MissionInfo>();
        }

        void ISaver<DeckInfo>.Save(DeckInfo value, int id)
        {
            gameInfo.Decks ??= new Dictionary<int, DeckInfo>();
            gameInfo.Decks[id] = value;
            UpdateYGSave();
        }

        DeckInfo IDownloader<DeckInfo>.Load(int id)
        {
            if (!isLoaded)
                throw new InvalidOperationException();
            if (gameInfo.Decks == null || !gameInfo.Decks.ContainsKey(id))
                return null;
            return gameInfo.Decks[id];
        }

        IEnumerable<DeckInfo> IAllDownloader<DeckInfo>.LoadAll()
        {
            if (!isLoaded)
                throw new InvalidOperationException();
            return gameInfo.Decks?.Values ?? Enumerable.Empty<DeckInfo>();
        }

        void ISaver<UserInfo>.Save(UserInfo value, int id)
        {
            gameInfo.UserInfo = value;
            UpdateYGSave();
        }

        UserInfo IDownloader<UserInfo>.Load(int id)
        {
            if (!isLoaded)
                throw new InvalidOperationException();
            return gameInfo.UserInfo;
        }

        IEnumerable<UserInfo> IAllDownloader<UserInfo>.LoadAll()
        {
            if (!isLoaded)
                throw new InvalidOperationException();
            if (gameInfo.UserInfo == null)
                yield break;
            yield return gameInfo.UserInfo;
        }
        
        private void UpdateYGSave()
        {
            YandexGame.savesData = YandexGame.savesData.UpdateSave(gameInfo);
            YandexGame.SaveProgress();
        }
    }
}