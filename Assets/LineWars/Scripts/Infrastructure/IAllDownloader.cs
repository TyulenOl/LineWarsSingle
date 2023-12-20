using System.Collections.Generic;

namespace LineWars.Model
{
    public interface IAllDownloader<out TValue>
    {
        public IEnumerable<TValue> LoadAll();
    }
}
