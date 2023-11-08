using System;

namespace LineWars.Model
{
    public interface IExecutor
    {
        public int MaxActionPoints { get; set; }
        public int CurrentActionPoints { get; set; }
        public event Action AnyActionCompleted;

        public bool CanDoAnyAction => CurrentActionPoints > 0;

        public bool TryGetCommandForTarget(CommandType priorityType, ITarget target,
            out IActionCommand command);

        public T Accept<T>(IExecutorVisitor<T> visitor);
    }

    public interface IExecutor<TExecutor, in TAction> : IExecutor
        where TExecutor : IExecutor<TExecutor, TAction>
        where TAction : IExecutorAction<TExecutor>
    {
        public bool TryGetUnitAction<T>(out T action) where T : TAction;
        public T GetUnitAction<T>() where T : TAction;
    }
}