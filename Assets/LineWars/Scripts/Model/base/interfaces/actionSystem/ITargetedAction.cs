using System;

namespace LineWars.Model
{
    public interface ITargetedAction : IExecutorAction
    {
        public bool IsAvailable(ITarget target);
    }

    public interface ITargetedActionCommandGenerator
    {
        public IActionCommand GenerateCommand(ITarget target);
    }

    public interface ITargetedAction<in TTarget> :
        ITargetedAction,
        ITargetedActionCommandGenerator
        where TTarget : ITarget
    {
        public bool IsAvailable(TTarget target);
        public void Execute(TTarget target);
        public IActionCommand GenerateCommand(TTarget target);


        bool ITargetedAction.IsAvailable(ITarget target)
        {
            return target is TTarget currentTarget && IsAvailable(currentTarget);
        }

        IActionCommand ITargetedActionCommandGenerator.GenerateCommand(ITarget target)
        {
            return GenerateCommand((TTarget) target);
        }
    }
}