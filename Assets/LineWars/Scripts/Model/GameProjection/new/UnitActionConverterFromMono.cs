using System;

namespace LineWars.Model
{
    public static class UnitActionConverterFromMono<TNode, TEdge, TUnit, TOwned, TPlayer>
        #region Constraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion
    {
        public static UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer> Convert(TUnit unit, MonoUnitAction action)
        {
            throw new NotImplementedException("There is no convertion for this action!");
        }

        public static BuildAction<TNode, TEdge, TUnit, TOwned, TPlayer> Convert(TUnit unit, MonoBuildRoadAction action)
        {
            var newAction =
                new BuildAction<TNode, TEdge, TUnit, TOwned, TPlayer>(unit, action);
            return newAction;
        }

        public static BlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> Convert(TUnit unit, MonoBlockAction action)
        {
            var newAction =
                new BlockAction<TNode, TEdge, TUnit, TOwned, TPlayer>(unit, action);
            return newAction;
        }

        public static MoveAction<TNode, TEdge, TUnit, TOwned, TPlayer> Convert(TUnit unit, MonoMoveAction action)
        {
            var newAction =
                new MoveAction<TNode, TEdge, TUnit, TOwned, TPlayer>(unit, action);
            return newAction;
        }

        public static HealAction<TNode, TEdge, TUnit, TOwned, TPlayer> Convert(TUnit unit, MonoHealAction action)
        {
            var newAction =
                new HealAction<TNode, TEdge, TUnit, TOwned, TPlayer>(unit, action);
            return newAction;
        }
    }
}
