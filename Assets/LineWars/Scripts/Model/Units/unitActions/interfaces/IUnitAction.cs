﻿namespace LineWars.Model
{
    public interface IUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>: 
        IExecutorAction<TUnit>
        
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TOwned, TPlayer>
        #endregion

    {
        public TUnit MyUnit => Executor;
        public uint GetPossibleMaxRadius();

        public TResult Accept<TResult>(IIUnitActionVisitor<TResult, TNode, TEdge, TUnit, TOwned, TPlayer> visitor);
    }
}