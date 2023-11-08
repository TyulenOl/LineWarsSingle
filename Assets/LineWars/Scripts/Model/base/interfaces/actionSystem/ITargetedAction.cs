using System;

namespace LineWars.Model
{
    public interface ITargetedAction
    {
        public Type TargetType { get; }
        public bool IsMyTarget(ITarget target);
        public IActionCommand GenerateCommand(ITarget target);
    }

    public interface ITargetedAction<in TTarget> : ITargetedAction
    {
        public bool CanExecute(TTarget target);
        public void Execute(TTarget target);
        public IActionCommand GenerateCommand(TTarget target);

        Type ITargetedAction.TargetType => typeof(TTarget);
        bool ITargetedAction.IsMyTarget(ITarget target) => target is TTarget;
        IActionCommand ITargetedAction.GenerateCommand(ITarget target) => GenerateCommand((TTarget) target);
    }
}