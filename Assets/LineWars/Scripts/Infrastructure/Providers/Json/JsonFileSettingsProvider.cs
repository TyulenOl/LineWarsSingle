using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "Providers/Json/JsonFileSettingsProvider", order = 52)]
    public class JsonFileSettingsProvider : SettingsProvider
    {
        private ISaver<Settings> saver;
        private IDownloader<Settings> downloader;
        private IAllDownloader<Settings> allDownloader;

        public void Initialize()
        {
            saver = new JsonFileSaver<Settings>();
            downloader = new JsonFileLoader<Settings>();
            allDownloader = new JsonFileAllDownloader<Settings>();
        }

        public override void Save(Settings value, int id) => saver.Save(value, id);
        public override Settings Load(int id) => downloader.Load(id);
        public override IEnumerable<Settings> LoadAll() => allDownloader.LoadAll();
    }
}