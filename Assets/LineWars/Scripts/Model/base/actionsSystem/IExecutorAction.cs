using System;

namespace LineWars.Model
{
    public interface IExecutorAction
    {
        public CommandType CommandType { get; }
        public void OnReplenish();
        public int GetActionPointsCost();
        public event Action ActionCompleted;
    }


    public interface IExecutorAction<out TExecutor> :
        IExecutorAction
        where TExecutor : IExecutor
    {
        public TExecutor Executor { get; }
    }
}