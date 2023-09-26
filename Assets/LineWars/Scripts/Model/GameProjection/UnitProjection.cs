using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public class UnitProjection : IReadOnlyUnitProjection
    {
        public ComponentUnit Unit { get; private set; }
        public Node Node { get; set; }
        public int CurrentHp { get; set; }
        public int CurrentArmor { get; set; }
        public int CurrentActionPoints { get; set; }
        public List<UnitActionProjection> Actions { get; set; }
        public IReadOnlyList<IReadOnlyUnitActionProjection> ActionProjections => Actions;

        public UnitProjection(ComponentUnit unit)
        {
            Unit = unit;
            Node = unit.Node;
            CurrentHp = unit.CurrentHp;
            CurrentArmor = unit.CurrentArmor;
            CurrentActionPoints = unit.CurrentActionPoints;
            Actions = unit.RuntimeActions
                .Select(action => UnitActionProjection.GetActionProjection(action)).ToList();
        }

        public UnitProjection(IReadOnlyUnitProjection projection)
        {
            Unit = projection.Unit;
            Node = projection.Node;
            CurrentHp = projection.CurrentHp;
            CurrentArmor = projection.CurrentArmor;
            CurrentActionPoints = projection.CurrentActionPoints;
            Actions = projection.ActionProjections
                .Select(action => UnitActionProjection.GetActionProjection(action)).ToList();
        }

        public T GetAction<T>() where T : UnitActionProjection => Actions.OfType<T>().FirstOrDefault();
        public bool TryGetAction<T>(out T action) where T : UnitActionProjection
        {
            action = GetAction<T>();
            return action != null;
        }
    }

    public interface IReadOnlyUnitProjection
    {
        public ComponentUnit Unit { get; }
        public Node Node { get; }
        public int CurrentHp { get; }
        public int CurrentArmor { get; }
        public int CurrentActionPoints { get; }
        public IReadOnlyList<IReadOnlyUnitActionProjection> ActionProjections { get; }
        public T GetReadOnlyAction<T>() where T : IReadOnlyUnitActionProjection 
            => ActionProjections.OfType<T>().FirstOrDefault();
        public bool TryGetReadOnlyAction<T>(out T action) where T : IReadOnlyUnitActionProjection
        {
            action = GetReadOnlyAction<T>();
            return action != null;
        }
    }
}
