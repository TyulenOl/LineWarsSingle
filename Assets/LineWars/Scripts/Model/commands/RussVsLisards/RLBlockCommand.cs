using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class RLBlockCommand<TNode, TEdge, TUnit> :
        ActionCommand<TUnit, IRLBlockAction<TNode, TEdge, TUnit>>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public RLBlockCommand([NotNull] TUnit executor) : base(executor) {}

        public RLBlockCommand([NotNull] IRLBlockAction<TNode, TEdge, TUnit> action) : base(action) {}

        public override void Execute()
        {
            Action.EnableBlock();
        }

        public override bool CanExecute()
        {
            return Action.CanBlock();
        }

        public override string GetLog()
        {
            return $"Юнит {Executor} встал в защиту";
        }
    }
}