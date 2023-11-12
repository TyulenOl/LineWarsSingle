using System;

namespace LineWars.Model
{
    public interface IExecutorAction
    {
        public CommandType CommandType { get; }
        public ActionType ActionType { get; }

        public event Action ActionCompleted;
        public void OnReplenish();
    }


    public interface IExecutorAction<out TExecutor> :
        IExecutorAction
        where TExecutor : IExecutor
    {
        public TExecutor Executor { get; }
    }
}