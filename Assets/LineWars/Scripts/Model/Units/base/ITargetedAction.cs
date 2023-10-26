using System;

namespace LineWars.Model
{
    public interface ITargetedAction
    {
        public Type TargetType { get; }
        public bool IsMyTarget(ITarget target);
        public ICommandWithCommandType GenerateCommand(ITarget target);
    }
}