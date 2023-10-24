using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    public interface IReadOnlyExecutor
    {
        public int MaxActionPoints { get; }
        public int CurrentActionPoints {get; }
        public bool CanDoAnyAction => CurrentActionPoints > 0;
        public event Action AnyActionCompleted;
        public event Action<IExecutorAction> CurrentActionCompleted;
        
        public bool TryGetCommand(CommandType priorityType, IReadOnlyTarget target, out ICommand command);
        public IEnumerable<(IReadOnlyTarget, CommandType)> GetAllAvailableTargets();
    }
}