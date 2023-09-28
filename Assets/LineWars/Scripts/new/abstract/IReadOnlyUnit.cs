using System;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public interface IReadOnlyUnit : INumbered, IReadOnlyOwned, IAlive, IReadOnlyExecutor, IReadOnlyTarget
    {
        public string UnitName { get; }
        public int MaxHp { get; }
        public int CurrentHp { get; }

        public int MaxArmor { get; }
        public int CurrentArmor { get; }

        public int Visibility { get; }

        public UnitType Type { get; }
        public UnitSize Size { get; }
        public LineType MovementLineType { get; }
        public IReadOnlyNode Node { get; }


        public bool IsDied => CurrentHp <= 0;

        public bool CanMoveOnLineWithType(LineType lineType) => lineType >= MovementLineType;

        public bool TryGetNeighbour([NotNullWhen(true)] out IReadOnlyUnit neighbour)
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

        public bool IsNeighbour(IReadOnlyUnit unit)
        {
            if (unit == this) return false;
            return Node.LeftUnit == unit || Node.RightUnit == unit;
        }
    }
}