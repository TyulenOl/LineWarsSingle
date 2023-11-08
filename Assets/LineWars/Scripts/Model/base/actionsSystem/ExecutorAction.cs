using System;
using JetBrains.Annotations;
using LineWars.Controllers;

namespace LineWars.Model
{
    public abstract class ExecutorAction<T>: IExecutorAction<T>
        where T: class, IExecutor 
    {
        public T Executor { get; }
        public IntModifier ActionModifier { get; set; }
        public event Action ActionCompleted;
        public abstract CommandType CommandType { get; }
        public abstract ActionType ActionType { get; }

        protected ExecutorAction(T executor)
        {
            Executor = executor;
        }

        public virtual void OnReplenish() {}
        
        protected void Complete() => ActionCompleted?.Invoke();
        
        protected void CompleteAndAutoModify()
        {
            Executor.CurrentActionPoints = ModifyActionPoints();
            Complete();
        }

        protected int ModifyActionPoints(int actionPoints) => ActionModifier.Modify(actionPoints);
        protected int ModifyActionPoints() => ModifyActionPoints(Executor.CurrentActionPoints);
        
        protected bool ActionPointsCondition(int actionPoints) => ActionPointsCondition(ActionModifier, actionPoints);
        protected bool ActionPointsCondition() => ActionPointsCondition(ActionModifier, Executor.CurrentActionPoints);
        
        protected static bool ActionPointsCondition(IntModifier modifier, int actionPoints) =>
            actionPoints > 0 && modifier != null && modifier.Modify(actionPoints) >= 0;
    }
}