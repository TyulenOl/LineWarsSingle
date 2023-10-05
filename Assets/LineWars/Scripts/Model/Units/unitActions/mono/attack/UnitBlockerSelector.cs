using System.Linq;
using LineWars.Scripts;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New UnitBlockerSelector", menuName = "BlockerSelectors/Base UnitBlockerSelector",
        order = 60)]
    public class UnitBlockerSelector : ScriptableObject
    {
        public virtual TUnit SelectBlocker<TNode, TEdge, TUnit, TOwned, TPlayer>(TUnit targetUnit, TUnit neighborUnit)
            where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TPlayer : class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer>
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