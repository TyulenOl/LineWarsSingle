using System;

namespace LineWars.Model
{
    public interface IMultiTargetedAction : ITargetedAction
    {
        public int TargetsCount { get; }
        public bool IsAvailable(params ITarget[] target);

        bool ITargetedAction.IsAvailable(ITarget target)
        {
            return IsAvailable(target);
        }
    }

    public interface IMultiTargetedActionGenerator
    {
        public IActionCommand GenerateCommand(params ITarget[] targets);
    }

    public interface IMultiTargetedAction<in TTarget1, in TTarget2> :
        IMultiTargetedAction,
        IMultiTargetedActionGenerator
    {
        public bool IsAvailable(TTarget1 target1);
        public bool IsAvailable(TTarget1 target1, TTarget2 target2);


        public bool CanExecute(TTarget1 target1, TTarget2 target2)
        {
            return IsAvailable(target1, target2);
        }

        public void Execute(TTarget1 target1, TTarget2 target2);
        public IActionCommand GenerateCommand(TTarget1 target1, TTarget2 target2);


        int IMultiTargetedAction.TargetsCount => 2;

        IActionCommand IMultiTargetedActionGenerator.GenerateCommand(params ITarget[] targets)
        {
            return GenerateCommand((TTarget1) targets[0], (TTarget2) targets[1]);
        }

        bool IMultiTargetedAction.IsAvailable(params ITarget[] target)
        {
            return target.Length switch
            {
                1 => target[0] is TTarget1 target1 && IsAvailable(target1),
                2 => target[0] is TTarget1 target1 && target[1] is TTarget2 target2 && IsAvailable(target1, target2),
                _ => throw new ArgumentOutOfRangeException(nameof(target.Length))
            };
        }
    }
}