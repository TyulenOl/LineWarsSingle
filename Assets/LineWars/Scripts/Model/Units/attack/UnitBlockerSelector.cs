using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New UnitBlockerSelector", menuName = "BlockerSelectors/Base UnitBlockerSelector", order = 60)]
    public class UnitBlockerSelector: ScriptableObject
    {
        // как насчет ввести простую систему с очками блока, у кого их больше тот и блокирует удар.
        public virtual ModelComponentUnit SelectBlocker(ModelComponentUnit targetUnit, ModelComponentUnit neighborUnit)
        {
            return
                (new[] {targetUnit, neighborUnit})
                .OrderByDescending(x => x.GetIsBlocked()) // Сперва берем того, кто блокирует
                .ThenByDescending(x => x.MaxHp) // Потом того, у кого больше максимальное хп
                .ThenByDescending(x => x.CurrentArmor) // Потом того, у кого больше армор
                .ThenBy(x=> x.CurrentHp) // Потом того, у кого меньше текущее хп
                .First();
        }
    }
}