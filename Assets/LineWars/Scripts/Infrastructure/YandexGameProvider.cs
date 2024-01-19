using System.Collections.Generic;
using YG;

namespace LineWars.Model
{
    public class YandexGameProvider:
        IProvider<MissionInfo>,
        IProvider<DeckInfo>,
        IProvider<UserInfo>,
        IProvider<Settings>
    {
        private void DownLoadAll()
        {
        }

        void ISaver<MissionInfo>.Save(MissionInfo value, int id)
        {
            throw new System.NotImplementedException();
        }

        MissionInfo IDownloader<MissionInfo>.Load(int id)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<MissionInfo> IAllDownloader<MissionInfo>.LoadAll()
        {
            throw new System.NotImplementedException();
        }

        void ISaver<DeckInfo>.Save(DeckInfo value, int id)
        {
            throw new System.NotImplementedException();
        }

        DeckInfo IDownloader<DeckInfo>.Load(int id)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<DeckInfo> IAllDownloader<DeckInfo>.LoadAll()
        {
            throw new System.NotImplementedException();
        }

        void ISaver<UserInfo>.Save(UserInfo value, int id)
        {
            throw new System.NotImplementedException();
        }

        UserInfo IDownloader<UserInfo>.Load(int id)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<UserInfo> IAllDownloader<UserInfo>.LoadAll()
        {
            throw new System.NotImplementedException();
        }

        void ISaver<Settings>.Save(Settings value, int id)
        {
            throw new System.NotImplementedException();
        }

        Settings IDownloader<Settings>.Load(int id)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<Settings> IAllDownloader<Settings>.LoadAll()
        {
            throw new System.NotImplementedException();
        }
    }
}