using System;
using JetBrains.Annotations;
using LineWars.Controllers;

namespace LineWars.Model
{
    public abstract class ExecutorAction: IExecutorAction
    {
        private readonly IntModifier actionModifier;
        private readonly IExecutor myExecutor;
        public event Action ActionCompleted;

        protected ExecutorAction([NotNull] IExecutor unit, [NotNull] MonoExecutorAction data)
        {
            actionModifier = data.ActionModifier;
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

        protected int ModifyActionPoints(int actionPoints) => actionModifier.Modify(actionPoints);
        protected int ModifyActionPoints() => ModifyActionPoints(myExecutor.CurrentActionPoints);
        
        protected bool ActionPointsCondition(int actionPoints) => ActionPointsCondition(actionModifier, actionPoints);
        protected bool ActionPointsCondition() => ActionPointsCondition(actionModifier, myExecutor.CurrentActionPoints);
        
        protected static bool ActionPointsCondition(IntModifier modifier, int actionPoints) =>
            actionPoints > 0 && modifier != null && modifier.Modify(actionPoints) >= 0;
    }
}