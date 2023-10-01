using System;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [RequireComponent(typeof(IExecutor))]
    public abstract class MonoExecutorAction : MonoBehaviour, IExecutorAction
    {
        protected ExecutorAction ExecutorAction;
        [SerializeField] protected IntModifier actionModifier;
        public event Action ActionCompleted;
        public IntModifier ActionModifier => actionModifier;

        void Awake()
        {
            ExecutorAction = GetAction();
            ExecutorAction.ActionCompleted += () => ActionCompleted?.Invoke();
        }

        protected abstract ExecutorAction GetAction();
        public CommandType GetMyCommandType() => ExecutorAction.GetMyCommandType();
        public void OnReplenish() => ExecutorAction.OnReplenish();

    }
}