using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public static class UnitExtension
    {
        public static int GetMaxDamage(this Unit unit)
        {
            return UnitUtilities<Node, Edge, Unit>.GetMaxDamage(unit);
        }

        public static IEnumerable<(CommandType, int)> GetDamages(this Unit unit)
        {
            return UnitUtilities<Node, Edge, Unit>.GetDamages(unit);
        }

        public static bool IsVisible(this Unit unit)
        {
            return unit.Node.IsVisible;
        }
    }
}