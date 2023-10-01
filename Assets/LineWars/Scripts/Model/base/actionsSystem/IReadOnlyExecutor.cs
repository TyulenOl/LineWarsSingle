using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    public interface IReadOnlyExecutor
    {
        public int CurrentActionPoints {get; }
        public bool CanDoAnyAction { get; }
        public event Action AnyActionCompleted;
        public event Action<ExecutorAction> CurrentActionCompleted;
        
        public bool TryGetCommand(CommandType priorityType, IReadOnlyTarget target, out ICommand command);
        public IEnumerable<(IReadOnlyTarget, CommandType)> GetAllAvailableTargets();
    }
}