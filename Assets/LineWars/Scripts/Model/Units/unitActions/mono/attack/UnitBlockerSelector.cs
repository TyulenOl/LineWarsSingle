using System.Linq;
using LineWars.Scripts;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New UnitBlockerSelector", menuName = "BlockerSelectors/Base UnitBlockerSelector", order = 60)]
    public class UnitBlockerSelector : ScriptableObject
    {
        public virtual TUnit SelectBlocker<TNode, TEdge, TUnit, TOwned, TPlayer>(TUnit targetUnit, TUnit neighborUnit)
            #region Сonstraints
            where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
            where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TOwned : class, IOwned<TOwned, TPlayer>
            where TPlayer: class, IBasePlayer<TOwned, TPlayer>
            #endregion 
        {
            return
                (new[] {targetUnit, neighborUnit})
                .OrderByDescending(x => x.CurrentArmor) // Потом того, у кого больше армор
                .ThenByDescending(x => x.MaxHp) // Потом того, у кого больше максимальное хп
                .ThenBy(x => x.CurrentHp) // Потом того, у кого меньше текущее хп
                .First();
        }
    }
}