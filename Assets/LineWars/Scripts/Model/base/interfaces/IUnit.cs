using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;


namespace LineWars.Model
{
    public interface IUnit<TNode, TEdge, TUnit> :
        IOwned,
        ITargetedAlive,
        IExecutor<TUnit, IUnitAction<TNode, TEdge, TUnit>>,
        IExecutorActionSource
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public int Id { get; }
        public int MaxArmor { get; set; }
        public int CurrentArmor { get; set; }
        public int Visibility { get; set; }
        public UnitType Type { get; }
        public UnitSize Size { get; }
        public LineType MovementLineType { get; }
        public UnitDirection UnitDirection { get; set; }
        public TNode Node { get; set; }

        public IEnumerable<IUnitAction<TNode, TEdge, TUnit>> Actions { get; }

        public void DealDamageThroughArmor(int value)
        {
            if (value < 0)
                throw new ArgumentException();
            if (value == 0)
                return;

            var blockedDamage = Mathf.Min(value, CurrentArmor);
            var notBlockedDamage = value - blockedDamage;

            CurrentArmor -= blockedDamage;
            CurrentHp -= notBlockedDamage;
        }

        public bool IsDied => CurrentHp <= 0;
        public bool CanMoveOnLineWithType(LineType lineType) => lineType >= MovementLineType;

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