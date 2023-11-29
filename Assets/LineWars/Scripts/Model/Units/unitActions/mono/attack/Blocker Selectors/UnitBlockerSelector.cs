using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New UnitBlockerSelector", menuName = "BlockerSelectors/Base UnitBlockerSelector", order = 60)]
    public class UnitBlockerSelector : ScriptableObject
    {
        public virtual TUnit SelectBlocker<TNode, TEdge, TUnit>(TUnit targetUnit, TUnit neighborUnit)
            #region Сonstraints
            where TNode : class, INodeForGame<TNode, TEdge, TUnit>
            where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit> 
            where TUnit : class, IUnit<TNode, TEdge, TUnit>
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