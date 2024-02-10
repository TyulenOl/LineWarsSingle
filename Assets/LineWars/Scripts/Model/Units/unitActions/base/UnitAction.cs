using System;
using System.Diagnostics.CodeAnalysis;


namespace LineWars.Model
{
    public abstract class UnitAction<TNode, TEdge, TUnit> :
        ExecutorAction<TUnit>,
        IUnitAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit> 
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        protected UnitAction(TUnit executor) : base(executor)
        {
        }

        public abstract void Accept(IBaseUnitActionVisitor<TNode, TEdge, TUnit> visitor);
        public abstract TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor);
    }
}