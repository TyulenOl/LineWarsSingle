using System.Collections.Generic;

namespace LineWars.Controllers
{ 
    public interface IDownloader<out TValue>
    {
        public TValue Load(int id);
    }
}
