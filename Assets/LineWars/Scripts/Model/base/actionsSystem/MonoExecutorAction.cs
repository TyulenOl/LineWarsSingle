using System;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [RequireComponent(typeof(IExecutor))]
    public abstract class MonoExecutorAction : MonoBehaviour, IExecutorAction
    {
        [field:SerializeField] public int InitializePriority { get; private set; }
        [SerializeField] protected IntModifier actionModifier;
        
        protected IExecutor Executor;
        protected ExecutorAction ExecutorAction;
        public event Action ActionCompleted;
        public IntModifier ActionModifier => actionModifier;

        public void Initialize()
        {
            Executor = GetComponent<IExecutor>();
            ExecutorAction = GetAction();
            ExecutorAction.ActionCompleted += () => ActionCompleted?.Invoke();
        }
        public void OnReplenish() => ExecutorAction.OnReplenish();
        public CommandType GetMyCommandType() => ExecutorAction.GetMyCommandType();
        protected abstract ExecutorAction GetAction();
    }
}