using LineWars.Model;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars
{
    public abstract class ProviderBase : MonoBehaviour,
        IProvider<MissionInfo>,
        IProvider<DeckInfo>,
        IProvider<UserInfo>
    {
        public abstract void Save(MissionInfo value, int id);
        public abstract void Save(DeckInfo value, int id);
        public abstract void Save(UserInfo value, int id);
        public abstract DeckInfo LoadDeckInfo(int id);
        public abstract IEnumerable<DeckInfo> LoadAllDeckInfo();
        public abstract UserInfo LoadUserInfo(int id);
        public abstract IEnumerable<UserInfo> LoadAllUserInfo();
        public abstract MissionInfo LoadMissionInfo(int id); 
        public abstract IEnumerable<MissionInfo> LoadAllMissionInfo();


        DeckInfo IDownloader<DeckInfo>.Load(int id)
        {
            return LoadDeckInfo(id);
        }

        IEnumerable<DeckInfo> IAllDownloader<DeckInfo>.LoadAll()
        {
            return LoadAllDeckInfo();
        }

        UserInfo IDownloader<UserInfo>.Load(int id)
        {
            return LoadUserInfo(id);
        }

        IEnumerable<UserInfo> IAllDownloader<UserInfo>.LoadAll()
        {
            return LoadAllUserInfo();
        }

        MissionInfo IDownloader<MissionInfo>.Load(int id)
        {
            return LoadMissionInfo(id);
        }

        IEnumerable<MissionInfo> IAllDownloader<MissionInfo>.LoadAll()
        {
            return LoadAllMissionInfo();
        }
    }
}
