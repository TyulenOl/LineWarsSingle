using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "Providers/Json/JsonFileUserInfoProvider", order = 52)]
    public class JsonFileUserInfoProvider: UserInfoProvider
    {
        private ISaver<UserInfo> saver;
        private IDownloader<UserInfo> downloader;
        private IAllDownloader<UserInfo> allDownloader;

        public void Initialize()
        {
            saver = new JsonFileSaver<UserInfo>();
            downloader = new JsonFileLoader<UserInfo>();
            allDownloader = new JsonFileAllDownloader<UserInfo>();
        }

        public override void Save(UserInfo value, int id) => saver.Save(value, id);
        public override UserInfo Load(int id) => downloader.Load(id);
        public override IEnumerable<UserInfo> LoadAll() => allDownloader.LoadAll();
    }
}