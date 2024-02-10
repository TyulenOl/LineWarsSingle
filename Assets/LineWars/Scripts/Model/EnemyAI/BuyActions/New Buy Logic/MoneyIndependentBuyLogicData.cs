using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Money Independent Buy Logic", menuName = "EnemyAI/Buy Logic/Money Independent")]
    public class MoneyIndependentBuyLogicData : AIBuyLogicData
    {
        [SerializeField] private int unitsLimit;        
        [SerializeField] private List<PurchaseList> unitsPerRound;

        public int UnitsLimit => unitsLimit;
        public IReadOnlyList<PurchaseList> UnitsPerRound => unitsPerRound;

        public override AIBuyLogic CreateAILogic(EnemyAI player)
        {
            return new MoneyIndependentLogic(player, this);
        }

        #region Classes
        [System.Serializable]
        public class UnitPurchase
        {
            [SerializeField] private UnitType unitType;
            [SerializeField] private int quantity;

            public UnitType UnitType => unitType;
            public int Quantity => quantity;
        }

        [System.Serializable]
        public class PurchaseList : IReadOnlyList<UnitPurchase>
        {
            [SerializeField] private List<UnitPurchase> unitPurchases;
            public UnitPurchase this[int index] => unitPurchases[index];

            public int Count => unitPurchases.Count;

            public IEnumerator<UnitPurchase> GetEnumerator()
            {
                return unitPurchases.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        #endregion
    }

    public class MoneyIndependentLogic : AIBuyLogic
    { 
        private int currentRoundId = 0;
        private int currentNodeId = 0;
        private readonly EnemyAI player;
        private readonly MoneyIndependentBuyLogicData data;
        public MoneyIndependentLogic(EnemyAI player, MoneyIndependentBuyLogicData data)
        {
            this.player = player;
            this.data = data;
        }

        public override void CalculateBuy()
        {
            if (data.UnitsPerRound.Count == 0)
                return;
            foreach (var purchase in data.UnitsPerRound[currentRoundId])
            {
                var currentUnit = player.GetUnitPrefab(purchase.UnitType);
                var quantity = purchase.Quantity;
                var eligbleNodes = MonoGraph.Instance.Nodes
                .Where(node => node.IsQualifiedForSpawn(player))
                .ToList();

                while (quantity > 0 && AreNodesFree(eligbleNodes, currentUnit))
                {
                    currentNodeId = currentNodeId % eligbleNodes.Count;
                    if (data.UnitsLimit != 0 && player.MyUnits.Count() > data.UnitsLimit)
                    {
                        break;
                    }
                    var currentNode = eligbleNodes[currentNodeId];
                    if (BasePlayerUtility.CanSpawnUnit(currentNode, currentUnit))
                    {
                        player.SpawnUnit(currentNode, currentUnit);
                        quantity--;
                        eligbleNodes = MonoGraph.Instance.Nodes
                            .Where(node => node.IsQualifiedForSpawn(player))
                            .ToList();
                        if (eligbleNodes.Count <= 0)
                            break;
                    }

                    currentNodeId = (currentNodeId + 1) % eligbleNodes.Count;
                }
            }

            currentRoundId = (currentRoundId + 1) % data.UnitsPerRound.Count;
        }

        private bool AreNodesFree(IEnumerable<Node> nodes, Unit unit)
        {
            foreach (var node in nodes)
            {
                if (BasePlayerUtility.CanSpawnUnit(node, unit))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

