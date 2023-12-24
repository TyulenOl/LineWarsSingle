using System.Collections.Generic;

namespace LineWars.Model
{ 
    public interface IDownloader<out TValue>
    {
        public TValue Load(int id);
    }
}
