using System;

namespace LineWars.Model
{
    public static class UnitActionDuplicator<TNode, TEdge, TUnit, TOwned, TPlayer>
        #region Constraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion
    {
        public static UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer> 
            Duplicate(TUnit unit, UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            throw new NotImplementedException
                ($"Duplication for the {action.GetType().Name} is not implemented!");
        }

        public static UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer> 
            Duplicate(TUnit unit, BlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            return new BlockAction<TNode, TEdge, TUnit, TOwned, TPlayer>(unit, action);
        }

        public static UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>
            Duplicate(TUnit unit, BuildAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            return new BuildAction<TNode, TEdge, TUnit, TOwned, TPlayer>(unit, action);
        }

        public static UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>
            Duplicate(TUnit unit, HealAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            return new HealAction<TNode, TEdge, TUnit, TOwned, TPlayer>(unit, action);
        }

        public static UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>
            Duplicate(TUnit unit, MoveAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            return new MoveAction<TNode, TEdge, TUnit, TOwned, TPlayer>(unit, action);
        }
    }
}
