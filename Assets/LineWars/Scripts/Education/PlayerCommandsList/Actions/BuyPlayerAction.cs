using LineWars.Model;
using UnityEngine;

namespace LineWars.Education
{
    public class BuyPlayerAction: PlayerAction
    {
        [Header("")]
        [SerializeField] private Node buyNode;
        [SerializeField] private string presetName;

        public override bool CanSelectNode(Node node)
        {
            return buyNode.Equals(node);
        }

        public override bool CanSelectUnitBuyPreset(UnitBuyPreset preset)
        {
            return preset == null || preset.Name.Equals(presetName);
        }
    }
}