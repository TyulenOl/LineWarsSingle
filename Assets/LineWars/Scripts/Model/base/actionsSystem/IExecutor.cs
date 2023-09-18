using System;
using System.Collections.Generic;
using LineWars.Model;
using UnityEngine.Events;

namespace LineWars.Model
{
    public interface IExecutor
    {
        public bool CanDoAnyAction { get; }
        public int CurrentActionPoints {get;}

        public T GetExecutorAction<T>() where T : ExecutorAction;
        public bool TryGetExecutorAction<T>(out T action) where T : ExecutorAction;
        
        public UnityEvent AnyActionCompleted { get; }
        public IEnumerable<(ITarget, CommandType)> GetAllAvailableTargets();
    } 
}

        