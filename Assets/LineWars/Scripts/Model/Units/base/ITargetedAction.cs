using System;

namespace LineWars.Model
{
    public interface ITargetedAction
    {
        public Type TargetType { get; }
        public bool IsMyTarget(ITarget target);
        public ICommandWithCommandType GenerateCommand(ITarget target);
    }

    public interface ITargetedAction<in TTarget> : ITargetedAction
        where TTarget : ITarget
    {
        public bool CanExecute(TTarget target);
        public void Execute(TTarget target);
        public ICommandWithCommandType GenerateCommand(TTarget target);
    }
}