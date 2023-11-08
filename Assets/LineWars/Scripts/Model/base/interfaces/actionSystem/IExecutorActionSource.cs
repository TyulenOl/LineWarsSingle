using System.Collections.Generic;

namespace LineWars.Model
{
    public interface IExecutorActionSource
    {
        public IEnumerable<IExecutorAction<IExecutor>> Actions { get; }
    }
}