using System;

namespace LineWars.Controllers
{
    public interface ITimeIndexer
    {
        public DateTime this[string key] { get; set; }
        public bool ContainsKey(string key);
    }
}