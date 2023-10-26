﻿namespace LineWars.Model
{
    public interface IMonoExecutorAction<out TExecutor, out TAction> :
        IExecutorAction<TExecutor>

        where TExecutor : class, IExecutor
        where TAction: ExecutorAction<TExecutor>
    {
         public int Priority { get; }
         public TAction Action { get;}

         public void Initialize();
         public void OnReplenish();
    }
}