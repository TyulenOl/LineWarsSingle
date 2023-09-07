using System.Linq;

namespace LineWars.Model
{
    public class BaseUnitBlockerSelector : IUnitBlockerSelector
    {
        public Unit SelectBlocker(Unit targetUnit, Unit neighborUnit)
        {
            return
                (new[] {targetUnit, neighborUnit})
                .OrderByDescending(x => x.IsBlocked) // Сперва берем того, кто блокирует
                .ThenByDescending(x => x.MaxHp) // Потом того, у кого больше максимальное хп
                .ThenByDescending(x => x.CurrentArmor) // Потом того, у кого больше армор
                .ThenBy(x=> x.CurrentHp) // Потом того, у кого меньше текущее хп
                .First();
        }
    }
}