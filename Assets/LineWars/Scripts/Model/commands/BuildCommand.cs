using System;
using JetBrains.Annotations;


namespace LineWars.Model
{
    public class BuildCommand<TNode, TEdge, TUnit> :
        TargetActionCommand<TUnit, IBuildAction<TNode, TEdge, TUnit>, TEdge>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public BuildCommand(
            [NotNull] TUnit executor,
            [NotNull] TEdge target) : base(executor, target)
        {
        }

        public BuildCommand(
            [NotNull] IBuildAction<TNode, TEdge, TUnit> action,
            [NotNull] TEdge target) : base(action, target)
        {
        }

        public override string GetLog()
        {
            return $"Инженер {Executor} улучшил дорогу {Target}";
        }
    }
}