using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public abstract class UnitAction : ExecutorAction
    {
        public readonly IUnit MyUnit;

        public UnitAction([NotNull] IUnit unit, [NotNull] MonoUnitAction data) : base(unit, data)
        {
            MyUnit = unit;
        }

        public virtual uint GetPossibleMaxRadius() => (uint) MyUnit.CurrentActionPoints;
    }
}