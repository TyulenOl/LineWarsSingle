using System;

namespace LineWars.Model
{
    public interface IExecutorAction<out TExecutor>
        where TExecutor: IExecutor 
    {
        public TExecutor Executor { get; }
        public CommandType CommandType { get; }
        
        public event Action ActionCompleted;
        public void OnReplenish();
    }
}