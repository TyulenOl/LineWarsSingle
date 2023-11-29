using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class BlowWithSwingCommand<TNode, TEdge, TUnit> :
        TargetActionCommand<TUnit, IBlowWithSwingAction<TNode, TEdge, TUnit>, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public BlowWithSwingCommand(
            [NotNull] TUnit executor,
            [NotNull] TUnit target) : base(executor, target)
        {
        }

        public BlowWithSwingCommand(
            [NotNull] IBlowWithSwingAction<TNode, TEdge, TUnit> action,
            [NotNull] TUnit target) : base(action, target)
        {
        }

        public override string GetLog()
        {
            return $"Юнит {Executor} нанес круговую атаку";
        }
    }
}