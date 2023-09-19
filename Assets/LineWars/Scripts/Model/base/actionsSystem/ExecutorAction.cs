using System;
using JetBrains.Annotations;
using LineWars.Controllers;

namespace LineWars.Model
{
    public abstract class ExecutorAction
    {
        private readonly IntModifier actionModifier;
        private readonly IExecutor myExecutor;
        protected readonly SFXData ActionSfx;
        public event Action ActionCompleted;

        protected ExecutorAction([NotNull] IExecutor unit, [NotNull] BaseExecutorAction data)
        {
            actionModifier = data.ActionModifier;
            myExecutor = unit;
            ActionSfx = data.ActionSfx;
            
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (myExecutor == null) throw new ArgumentNullException(nameof(unit));
        }

        public abstract CommandType GetMyCommandType();

        public virtual void OnReplenish() {}
        
        protected void Complete() => ActionCompleted?.Invoke();

        public int ModifyActionPoints(int actionPoints) => actionModifier.Modify(actionPoints);
        public int ModifyActionPoints() => ModifyActionPoints(myExecutor.CurrentActionPoints);
        
        public bool ActionPointsCondition(int actionPoints) => ActionPointsCondition(actionModifier, actionPoints);
        public bool ActionPointsCondition() => ActionPointsCondition(actionModifier, myExecutor.CurrentActionPoints);
        
        public static bool ActionPointsCondition(IntModifier modifier, int actionPoints) =>
            actionPoints > 0 && modifier != null && modifier.Modify(actionPoints) >= 0;
    }
}