using System;
using JetBrains.Annotations;
using LineWars.Controllers;

namespace LineWars.Model
{
    public abstract class ExecutorAction: IExecutorAction
    {
        
        private readonly IExecutor myExecutor;
        public event Action ActionCompleted;
        public IntModifier ActionModifier { get; private set; }

        protected ExecutorAction([NotNull] IExecutor unit, [NotNull] MonoExecutorAction data)
        {
            ActionModifier = data.ActionModifier;
            myExecutor = unit;
            
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (myExecutor == null) throw new ArgumentNullException(nameof(unit));
        }

        protected ExecutorAction([NotNull] IExecutor unit, [NotNull] ExecutorAction data)
        {
            ActionModifier = data.ActionModifier;
            myExecutor = unit;

            if (data == null) throw new ArgumentNullException(nameof(data));
            if (myExecutor == null) throw new ArgumentNullException(nameof(unit));
        }

        public abstract CommandType GetMyCommandType();

        public virtual void OnReplenish() {}
        
        protected void Complete() => ActionCompleted?.Invoke();
        
        protected void CompleteAndAutoModify()
        {
            myExecutor.CurrentActionPoints = ModifyActionPoints();
            Complete();
        }

        protected int ModifyActionPoints(int actionPoints) => ActionModifier.Modify(actionPoints);
        protected int ModifyActionPoints() => ModifyActionPoints(myExecutor.CurrentActionPoints);
        
        protected bool ActionPointsCondition(int actionPoints) => ActionPointsCondition(ActionModifier, actionPoints);
        protected bool ActionPointsCondition() => ActionPointsCondition(ActionModifier, myExecutor.CurrentActionPoints);
        
        protected static bool ActionPointsCondition(IntModifier modifier, int actionPoints) =>
            actionPoints > 0 && modifier != null && modifier.Modify(actionPoints) >= 0;
    }
}