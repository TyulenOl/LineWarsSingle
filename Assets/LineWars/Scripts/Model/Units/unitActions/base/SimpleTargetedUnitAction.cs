using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public abstract class SimpleTargetedUnitAction<TNode, TEdge, TUnit, TTarget> :
        UnitAction<TNode, TEdge, TUnit>,
        ITargetedAction<TTarget>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
        where TTarget : ITarget
    {
        protected SimpleTargetedUnitAction([NotNull] TUnit unit) : base(unit) {}

        public abstract bool CanExecute(TTarget target);
        public abstract void Execute(TTarget target);
        public abstract ICommandWithCommandType GenerateCommand(TTarget target);
        
        public Type TargetType { get; } = typeof(TTarget);
        public bool IsMyTarget(ITarget target)
        {
            return target is TTarget;
        }

        public ICommandWithCommandType GenerateCommand(ITarget target)
        {
            return GenerateCommand((TTarget) target);
        }

    }
}