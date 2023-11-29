using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class MoveCommand<TNode, TEdge, TUnit> :
        TargetActionCommand<TUnit, IMoveAction<TNode, TEdge, TUnit>, TNode>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private readonly TNode start;

        public MoveCommand(
            [NotNull] TUnit executor,
            [NotNull] TNode target) : base(executor, target)
        {
            start = executor.Node;
        }

        public MoveCommand(
            [NotNull] IMoveAction<TNode, TEdge, TUnit> action,
            [NotNull] TNode target) : base(action,
            target)
        {
            start = action.Executor.Node;
        }

        public override string GetLog()
        {
            return $"Юнит {Executor} переместился из {start} в {Target}";
        }
    }
}