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
        public UnityEvent ActionCompleted { get; }
        
        public IEnumerable<(ITarget, CommandType)> GetAllAvailableTargets();
    } 
}

        