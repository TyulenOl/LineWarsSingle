using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    public interface IExecutor
    {
        public int MaxActionPoints { get; set; }
        public int CurrentActionPoints { get; set; }
        public IEnumerable<IExecutorAction> Actions { get; }
        public bool CanDoAnyAction => CurrentActionPoints > 0;
        public event Action ExecutorDestroyed;
    }

    public interface IExecutor<TExecutor, in TAction> : IExecutor
        where TExecutor : IExecutor<TExecutor, TAction>
        where TAction : IExecutorAction<TExecutor>
    {
        public bool TryGetAction<T>(out T action) where T : TAction;
        public T GetAction<T>() where T : TAction;
    }
}