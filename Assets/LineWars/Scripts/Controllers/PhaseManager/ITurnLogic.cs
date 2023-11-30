using System;

namespace LineWars.Model
{
    public interface ITurnLogic
    {
        public event Action<ITurnLogic> Ended;
        public void Start();     
    }
}
