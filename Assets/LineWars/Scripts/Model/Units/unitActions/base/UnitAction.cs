﻿using System.Diagnostics.CodeAnalysis;


namespace LineWars.Model
{
    public abstract class UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer> :
        ExecutorAction,
        IUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>
    
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer>
        #endregion 
    {
        public TUnit MyUnit { get; }

        public UnitAction([NotNull] TUnit unit, [NotNull] MonoUnitAction data) : base(unit, data)
        {
            MyUnit = unit;
        }

        public virtual uint GetPossibleMaxRadius() => (uint) MyUnit.CurrentActionPoints;
    }
}