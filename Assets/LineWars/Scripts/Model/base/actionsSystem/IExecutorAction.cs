using System;

namespace LineWars.Model
{
    public interface IExecutorAction
    {
        public event Action ActionCompleted;

        public CommandType GetMyCommandType();

        public void OnReplenish();
    }
}