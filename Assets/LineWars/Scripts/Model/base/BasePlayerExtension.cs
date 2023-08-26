using System.Linq;
using LineWars.Model;

namespace LineWars
{
    public static class BasePlayerExtension
    {
        public static int GetCountUnitByType(this BasePlayer player, UnitType type)
        {
            return player.OwnedObjects
                .OfType<Unit>()
                .Count(x => x.Type == type);
        }
    }
}