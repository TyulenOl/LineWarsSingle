
using System;
using System.Collections.Generic;
using LineWars.Model;
using UnityEngine.Events;

public interface IExecutor 
{
    public int CurrentActionPoints {get;}
    
    public UnityEvent ActionCompleted { get; }

    public IEnumerable<(ITarget, CommandType)> GetAllAvailableTargets();
}
    