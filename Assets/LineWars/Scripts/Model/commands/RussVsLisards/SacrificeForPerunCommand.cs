using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class SacrificeForPerunCommand<TNode, TEdge, TUnit> :
        TargetActionCommand<TUnit, ISacrificeForPerunAction<TNode, TEdge, TUnit>, TNode>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public SacrificeForPerunCommand(
            [NotNull] TUnit executor,
            [NotNull] TNode target) : base(executor, target) { }

        public SacrificeForPerunCommand(
            [NotNull] ISacrificeForPerunAction<TNode, TEdge, TUnit> action,
            [NotNull] TNode target) : base(action, target) { }

        public override string GetLog()
        {
            return $"Юнит {Executor} пожертвовал собой ради того," +
                   $" чтобы нанести урон всем в юнитам в ноде {Target}. Настоящий герой!";
        }
    }
}