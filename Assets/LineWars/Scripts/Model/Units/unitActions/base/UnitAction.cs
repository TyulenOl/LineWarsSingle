using System;
using System.Diagnostics.CodeAnalysis;


namespace LineWars.Model
{
    public abstract class UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer> :
        ExecutorAction<TUnit>,
        IUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>
    
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TOwned, TPlayer>
        #endregion

    {
        public TUnit MyUnit => Executor;
        
        public virtual uint GetPossibleMaxRadius() => (uint) MyUnit.CurrentActionPoints;

        public abstract void Accept(IUnitActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> visitor);

        protected UnitAction(TUnit executor) : base(executor)
        {
        }
    }
}