using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public interface IUnit: IOwned, IReadOnlyUnit, IExecutor, IAlive
    {
        public new int CurrentHp { get; set; }
        
        public new int CurrentArmor { get; set; }
        
        public new int Visibility { get; set;}

        public UnitDirection UnitDirection { get; set; }

        public new INode Node { get; set; }

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