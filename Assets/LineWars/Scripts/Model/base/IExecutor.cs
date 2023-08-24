
using System;
using System.Collections.Generic;
using LineWars.Model;

public interface IExecutor 
{
    public int CurrentActionPoints {get;}

    public IEnumerable<(ITarget, CommandType)> GetAllAvailableTargets();
}
    