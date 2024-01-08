using JetBrains.Annotations;
using System;

namespace LineWars.Model
{
    public class TargetedUniversalCommand<TExecutor, TAction, TTarget> :
        ActionCommand<TExecutor, TAction>
        where TExecutor : IExecutor<TExecutor, TAction>, IExecutor
        where TAction : IExecutorAction<TExecutor>, ITargetedAction<TTarget>
        where TTarget : ITarget
    {
        public TTarget Target { get; }

        public TargetedUniversalCommand(
            [NotNull] TExecutor executor,
            [NotNull] TTarget target) : base(executor)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
        }

        public TargetedUniversalCommand(
            [NotNull] TAction action,
            [NotNull] TTarget target) : base(action)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
        }

        public override void Execute()
        {
            Action.Execute(Target);
        }

        public override bool CanExecute()
        {
            return Action.IsAvailable(Target);
        }

        public override string GetLog()
        {
            return $"Executor: {Executor}, Target: {Target}, Action: {Action.CommandType}";
        }
    }
}
