using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class AttackCommand<TNode, TEdge, TUnit> :
        TargetActionCommand<TUnit, IAttackAction<TNode, TEdge, TUnit>, ITargetedAlive>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public AttackCommand(
            [NotNull] TUnit executor,
            [NotNull] ITargetedAlive target) : base(executor, target)
        {
        }

        public AttackCommand(
            [NotNull] IAttackAction<TNode, TEdge, TUnit> action,
            [NotNull] ITargetedAlive target) : base(action, target)
        {
        }

        public override string GetLog()
        {
            return $"{Executor} атаковал {Target}";
        }
    }
}