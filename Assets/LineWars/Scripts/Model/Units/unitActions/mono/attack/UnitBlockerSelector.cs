using System.Linq;
using LineWars.Scripts;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New UnitBlockerSelector", menuName = "BlockerSelectors/Base UnitBlockerSelector",
        order = 60)]
    public class UnitBlockerSelector : ScriptableObject
    {
        public virtual TUnit SelectBlocker<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>(TUnit targetUnit, TUnit neighborUnit)
            where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
            where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
            where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
            where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
            where TPlayer : class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
            where TNation : class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        {
            return
                (new[] {targetUnit, neighborUnit})
                //.OrderByDescending(x => x.GetIsBlocked()) // Сперва берем того, кто блокирует
                .OrderByDescending(x => x.MaxHp) // Потом того, у кого больше максимальное хп
                .ThenByDescending(x => x.CurrentArmor) // Потом того, у кого больше армор
                .ThenBy(x => x.CurrentHp) // Потом того, у кого меньше текущее хп
                .First();
        }
    }
}