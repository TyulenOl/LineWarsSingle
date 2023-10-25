using System;

namespace LineWars.Model
{
    public interface IExecutor
    {
        public int MaxActionPoints { get; set; }
        public int CurrentActionPoints { get; set; }

        public event Action AnyActionCompleted;
        public event Action<IExecutorAction> CurrentActionCompleted;

        public bool CanDoAnyAction => CurrentActionPoints > 0;
        public bool TryGetCommandForTarget(CommandType priorityType, ITarget target, out ICommandWithCommandType command);
        public T Accept<T>(IExecutorVisitor<T> visitor);
    }
}