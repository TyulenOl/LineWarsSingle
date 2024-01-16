using System;

namespace LineWars.Model
{
    public interface IExecutorAction
    {
        public CommandType CommandType { get; }
        public IExecutor Executor { get; }
        
        public void OnReplenish();
        public int GetActionPointsCost();
        public event Action ActionCompleted;
    }


    public interface IExecutorAction<out TExecutor> :
        IExecutorAction
        where TExecutor : IExecutor
    {
        public new TExecutor Executor { get; }
        IExecutor IExecutorAction.Executor => Executor;
    }
}