using System;
using System.Diagnostics.CodeAnalysis;


namespace LineWars.Model
{
    public abstract class UnitAction<TNode, TEdge, TUnit> :
        ExecutorAction<TUnit>,
        IUnitAction<TNode, TEdge, TUnit>
    
        #region Сonstraints
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit> 
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
        #endregion
    {
        public TUnit MyUnit => Executor;
        
        protected UnitAction(TUnit executor) : base(executor)
        {
        }
        
        public virtual uint GetPossibleMaxRadius() => (uint) MyUnit.CurrentActionPoints;

        public abstract void Accept(IBaseUnitActionVisitor<TNode, TEdge, TUnit> visitor);
        public abstract TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor);
    }
}