using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "new Straight Forward Selector", menuName = "BlockerSelectors/Straight Forward", order = 60)]
    public class StraightForwardBlockedSelector : UnitBlockerSelector
    {
        public override TUnit SelectBlocker<TNode, TEdge, TUnit>(TUnit targetUnit, TUnit neighborUnit)
        {
            return targetUnit;
        }
    }
}
