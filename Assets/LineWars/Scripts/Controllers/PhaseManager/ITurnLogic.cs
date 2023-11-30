using LineWars.Model;
using System;

namespace LineWars.Controllers
{
    public interface ITurnLogic
    {
        public event Action<ITurnLogic> Ended;
        public void Start();     
    }
}
