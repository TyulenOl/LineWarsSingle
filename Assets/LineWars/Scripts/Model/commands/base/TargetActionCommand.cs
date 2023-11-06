using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public abstract class TargetActionCommand<TExecutor, TAction, TTarget> : 
        ActionCommand<TExecutor, TAction>
        where TAction : IExecutorAction<TExecutor>, ITargetedAction<TTarget>
        where TExecutor : IExecutor<TExecutor, TAction>, IExecutor
        where TTarget: ITarget
    {
        public TTarget Target { get; }

        protected TargetActionCommand([NotNull] TExecutor executor, [NotNull] TTarget target) : base(executor)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
        }

        protected TargetActionCommand([NotNull] TAction action, [NotNull] TTarget target) : base(action)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
        }

        public override void Execute()
        {
            Action.Execute(Target);
        }

        public override bool CanExecute()
        {
            return Action.CanExecute(Target);
        }
    }
}