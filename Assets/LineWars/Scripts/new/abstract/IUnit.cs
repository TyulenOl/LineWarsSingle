using System.Diagnostics;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public interface IUnit: IOwned, IReadOnlyUnit, IExecutor, ITarget
    {
        public new int CurrentHp { get; set; }
        public new int CurrentArmor { get; set; }
        public new int Visibility { get; set;}
        public UnitDirection UnitDirection { get; set; }
        public new INode Node { get; set; }

        public void HealMe(int value)
        {
            if (value < 0) throw new ArgumentException("Heal value less than zero!");
            CurrentHp += value;
        }

        public bool TryGetNeighbour([NotNullWhen(true)] out IUnit neighbour)
        {
            if (TryGetNeighbour(out IReadOnlyUnit unit))
            {
                neighbour = (IUnit)unit;
                return true;
            }

            neighbour = null;
            return false;
        }
    }
}