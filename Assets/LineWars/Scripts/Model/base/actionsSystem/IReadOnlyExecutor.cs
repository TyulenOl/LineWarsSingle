using System;
using System.Collections.Generic;
using LineWars.Model;
using UnityEngine.Events;

namespace LineWars.Model
{
    public interface IReadOnlyExecutor
    {
        public int CurrentActionPoints {get; }
        public bool CanDoAnyAction { get; }
        public event Action AnyActionCompleted;
        public event Action<UnitAction> CurrentActionCompleted;

        public T GetExecutorAction<T>() where T : ExecutorAction;
        public bool TryGetExecutorAction<T>(out T action) where T : ExecutorAction;
        
        public bool TryGetCommand(CommandType priorityType, IReadOnlyTarget target, out ICommand command);
        public IEnumerable<(IReadOnlyTarget, CommandType)> GetAllAvailableTargets();
    } 
}

        