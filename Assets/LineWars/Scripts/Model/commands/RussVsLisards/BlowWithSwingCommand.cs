using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class BlowWithSwingCommand<TNode, TEdge, TUnit> :
        ActionCommand<TUnit, IBlowWithSwingAction<TNode, TEdge, TUnit>>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public BlowWithSwingCommand([NotNull] TUnit executor) : base(executor)
        {
        }

        public BlowWithSwingCommand([NotNull] IBlowWithSwingAction<TNode, TEdge, TUnit> action) : base(action)
        {
        }

        public override void Execute()
        {
            Action.ExecuteBlowWithSwing();
        }

        public override bool CanExecute()
        {
            return Action.CanBlowWithSwing();
        }

        public override string GetLog()
        {
            return $"Юнит {Executor} нанес круговую атаку";
        }
    }
}