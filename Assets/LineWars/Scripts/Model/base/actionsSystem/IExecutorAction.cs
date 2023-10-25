using System;

namespace LineWars.Model
{
    public interface IExecutorAction
    {
        public event Action ActionCompleted;

        public CommandType CommandType { get; }
    }
}