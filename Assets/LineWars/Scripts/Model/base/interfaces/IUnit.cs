using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace LineWars.Model
{
    public interface IUnit<TNode, TEdge, TUnit> :
        IOwned,
        ITargetedAlive,
        IExecutor<TUnit, IUnitAction<TNode, TEdge, TUnit>>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public int Id { get; }
        public int InitialPower {get; set;}
        public int CurrentPower {get; set; }
        public int MaxArmor { get; set; }
        public int CurrentArmor { get; set; }
        public int Visibility { get; set; }
        public UnitType Type { get; }
        public UnitSize Size { get; }
        public LineType MovementLineType { get; }
        public UnitDirection UnitDirection { get; set; }
        public TNode Node { get; set; }
        public IReadOnlyList<Effect<TNode, TEdge, TUnit>> Effects { get; }
        public new IEnumerable<IUnitAction<TNode, TEdge, TUnit>> Actions { get; }

        public event Action<TUnit, TNode, TNode> UnitNodeChanged;
        public event Action<TUnit, int, int> UnitHPChanged;
        public event Action<TUnit, int, int> UnitActionPointsChanged;
        public event Action<TUnit, int, int> UnitPowerChanged;
        public event Action<TUnit, int, int> UnitArmorChanged;
        public event Action<TUnit> UnitReplenished;
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

        public void AddEffect(Effect<TNode, TEdge, TUnit> effect);
        public void RemoveEffect(Effect<TNode, TEdge, TUnit> effect);
    }
}