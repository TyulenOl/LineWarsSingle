using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class HealCommand<TNode, TEdge, TUnit> :
        TargetActionCommand<TUnit, IHealAction<TNode, TEdge, TUnit>, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public HealCommand(
            [NotNull] TUnit executor,
            [NotNull] TUnit target) : base(executor, target)
        {
        }

        public HealCommand(
            [NotNull] IHealAction<TNode, TEdge, TUnit> action,
            [NotNull] TUnit target) : base(action, target)
        {
        }

        public override string GetLog()
        {
            return $"Доктор {Executor} похилил {Target}";
        }
    }
}