using System;

namespace LineWars.Model
{
    public interface IMultiTargetedAction : ITargetedAction
    {
        public int TargetsCount { get; }
        public bool IsAvailable(int targetID, ITarget target);

        bool ITargetedAction.IsAvailable(ITarget target)
        {
            return IsAvailable(0, target);
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
        public bool IsAvailable1(TTarget1 target1);
        public bool IsAvailable2(TTarget2 target2);


        public bool CanExecute(TTarget1 target1, TTarget2 target2)
        {
            return IsAvailable1(target1) && IsAvailable2(target2);
        }
        
        public void Execute(TTarget1 target1, TTarget2 target2);
        public IActionCommand GenerateCommand(TTarget1 target1, TTarget2 target2);


        int IMultiTargetedAction.TargetsCount => 2;

        IActionCommand IMultiTargetedActionGenerator.GenerateCommand(params ITarget[] targets)
        {
            return GenerateCommand((TTarget1) targets[0], (TTarget2) targets[1]);
        }

        bool IMultiTargetedAction.IsAvailable(int targetID, ITarget target)
        {
            return targetID switch
            {
                0 => target is TTarget1 target1 && IsAvailable1(target1),
                1 => target is TTarget2 target2 && IsAvailable2(target2),
                _ => throw new ArgumentOutOfRangeException(nameof(targetID))
            };
        }
    }
}