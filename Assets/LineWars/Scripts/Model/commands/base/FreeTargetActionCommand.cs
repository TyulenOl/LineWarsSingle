using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class FreeTargetActionCommand<TExecutor, TAction, TTarget> :
        TargetActionCommand<TExecutor, TAction, TTarget>
        where TExecutor : IExecutor<TExecutor, TAction>, IExecutor
        where TAction : IExecutorAction<TExecutor>, ITargetedAction<TTarget>
        where TTarget : ITarget
    {
        private Func<TExecutor, TTarget, string> GetLogFunction { get; }

        public FreeTargetActionCommand(
            [NotNull] TExecutor executor,
            [NotNull] TTarget target,
            [NotNull] Func<TExecutor, TTarget, string> getLogFunction) : base(executor, target)
        {
            GetLogFunction = getLogFunction ?? throw new ArgumentNullException(nameof(getLogFunction));
        }

        public FreeTargetActionCommand(
            [NotNull] TAction action,
            [NotNull] TTarget target,
            [NotNull] Func<TExecutor, TTarget, string> getLogFunction) : base(action, target)
        {
            GetLogFunction = getLogFunction ?? throw new ArgumentNullException(nameof(getLogFunction));
        }

        public override string GetLog()
        {
            return GetLogFunction(Executor, Target);
        }
    }
}