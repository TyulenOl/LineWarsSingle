using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class ShotUnitCommand<TNode, TEdge, TUnit> :
        MultiTargetedActionCommand<TUnit, IShotUnitAction<TNode, TEdge, TUnit>, TUnit, TNode>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public ShotUnitCommand(
            [NotNull] TUnit executor,
            [NotNull] TUnit target1,
            [NotNull] TNode target2) : base(executor, target1, target2)
        {
        }

        public ShotUnitCommand(
            [NotNull] IShotUnitAction<TNode, TEdge, TUnit> action,
            [NotNull] TUnit target1, [NotNull] TNode target2) : base(action, target1, target2)
        {
        }
        
        public override string GetLog()
        {
            return $"Юнит {Executor} бросил юнита {Target1} в ноду {Target2}";
        }
    }
}