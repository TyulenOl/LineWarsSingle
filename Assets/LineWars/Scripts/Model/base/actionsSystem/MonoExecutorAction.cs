using System;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [RequireComponent(typeof(IExecutor))]
    public abstract class MonoExecutorAction<TExecutor, TAction> :
        MonoBehaviour, 
        IMonoExecutorAction<TExecutor, TAction>

        where TExecutor : class, IExecutor
        where TAction: ExecutorAction<TExecutor>
    {
        [field: SerializeField] public int Priority { get; private set; }
        [SerializeField] protected IntModifier actionModifier;

        public TExecutor Executor { get; private set; }
        public TAction Action { get; private set; }
        public event Action ActionCompleted;

        public virtual void Initialize()
        {
            Executor = GetComponent<TExecutor>();
            Action = GetAction();
            Action.ActionModifier = actionModifier;
            Action.ActionCompleted += () => ActionCompleted?.Invoke();
        }

        public void OnReplenish() => Action.OnReplenish();

        public CommandType CommandType => Action.CommandType;
        public ActionType ActionType => Action.ActionType;

        protected abstract TAction GetAction();
    }
}