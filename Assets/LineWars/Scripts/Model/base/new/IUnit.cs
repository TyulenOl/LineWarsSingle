﻿using System.Diagnostics.CodeAnalysis;


namespace LineWars.Model
{
    public interface IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>: 
        INumbered,
        IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>,
        ITarget,
        IExecutor,
        IAlive
    
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TPlayer: class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNation: class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        #endregion
    {
        public string UnitName { get; }
        public int MaxHp { get; }

        public int MaxArmor { get; }
        public int CurrentArmor { get; set; }

        public int Visibility { get; }

        public UnitType Type { get; }
        public UnitSize Size { get; }
        public LineType MovementLineType { get; }
        public UnitDirection UnitDirection { get; set; }
        public TNode Node { get; set; }
        
        public bool IsDied => CurrentHp <= 0;

        public bool CanMoveOnLineWithType(LineType lineType) => lineType >= MovementLineType;

        public T GetUnitAction<T>() where T : IUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>;

        public bool TryGetUnitAction<T>(out T action) where T : IUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>;

        public bool TryGetNeighbour([NotNullWhen(true)] out TUnit neighbour)
        {
            neighbour = null;
            if (Size == UnitSize.Large)
                return false;
            if (Node.LeftUnit == this && Node.RightUnit != null)
            {
                neighbour = Node.RightUnit;
                return true;
            }

            if (Node.RightUnit == this && Node.LeftUnit != null)
            {
                neighbour = Node.LeftUnit;
                return true;
            }

            return false;
        }

        public bool IsNeighbour(TUnit unit)
        {
            if (unit == this) return false;
            return Node.LeftUnit == unit || Node.RightUnit == unit;
        }
    }
}