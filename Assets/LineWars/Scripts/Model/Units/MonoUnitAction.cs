using System;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public abstract class MonoUnitAction: MonoExecutorAction
    {
        [SerializeField] private SFXData actionSfx;

        public override ExecutorAction GetAction(IReadOnlyExecutor executor)
        {
            if (executor is not ComponentUnit unit)
                throw new ArgumentException($"{nameof(executor)} is not {nameof(ComponentUnit)}!");
            var action = GetAction(unit);
            action.ActionCompleted += () => SfxManager.Instance.Play(actionSfx);
            return action;
        }
        protected abstract UnitAction GetAction(ComponentUnit unit);
    }
}