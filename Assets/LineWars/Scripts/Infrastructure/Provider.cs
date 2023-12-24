using System.Collections.Generic;

namespace LineWars.Model
{
    public class Provider<T>: IProvider<T>
    {
        private readonly ISaver<T> saver;
        private readonly IDownloader<T> downloader;
        private readonly IAllDownloader<T> allDownloader;

        public Provider(ISaver<T> saver, IDownloader<T> downloader, IAllDownloader<T> allDownloader)
        {
            this.saver = saver;
            this.downloader = downloader;
            this.allDownloader = allDownloader;
        }


        public void Save(T value, int id)
        {
            saver.Save(value, id);
        }

        public T Load(int id)
        {
            return downloader.Load(id);
        }

        public IEnumerable<T> LoadAll()
        {
            return allDownloader.LoadAll();
        }
    }
}