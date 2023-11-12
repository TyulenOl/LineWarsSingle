using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public abstract class MultiTargetedActionCommand<TExecutor, TAction, TTarget1, TTarget2> :
        ActionCommand<TExecutor, TAction>
        where TExecutor : IExecutor<TExecutor, TAction>, IExecutor
        where TAction : IExecutorAction<TExecutor>, IMultiTargetedAction<TTarget1, TTarget2>
        where TTarget1 : ITarget
        where TTarget2 : ITarget
    {
        public TTarget1 Target1 { get; }
        public TTarget2 Target2 { get; }


        protected MultiTargetedActionCommand(
            [NotNull] TExecutor executor,
            [NotNull] TTarget1 target1,
            [NotNull] TTarget2 target2) : base(executor)
        {
            this.Target1 = target1 ?? throw new ArgumentNullException(nameof(target1));
            this.Target2 = target2 ?? throw new ArgumentNullException(nameof(target2));
        }

        protected MultiTargetedActionCommand(
            [NotNull] TAction action,
            [NotNull] TTarget1 target1,
            [NotNull] TTarget2 target2) : base(action)
        {
            this.Target1 = target1 ?? throw new ArgumentNullException(nameof(target1));
            this.Target2 = target2 ?? throw new ArgumentNullException(nameof(target2));
        }

        public override void Execute()
        {
            Action.Execute(Target1, Target2);
        }

        public override bool CanExecute()
        {
            return Action.CanExecute(Target1, Target2);
        }
    }
}