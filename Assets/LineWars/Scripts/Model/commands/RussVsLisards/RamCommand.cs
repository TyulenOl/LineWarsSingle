using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class RamCommand<TNode, TEdge, TUnit> :
        TargetActionCommand<TUnit, IRamAction<TNode, TEdge, TUnit>, TNode>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public RamCommand([NotNull] TUnit executor, [NotNull] TNode target) : base(executor, target){}

        public RamCommand([NotNull] IRamAction<TNode, TEdge, TUnit> action, [NotNull] TNode target) : base(action, target){}

        public override string GetLog()
        {
            return $"Юнит {Executor} протаранил ноду {Target}";
        }
    }
}