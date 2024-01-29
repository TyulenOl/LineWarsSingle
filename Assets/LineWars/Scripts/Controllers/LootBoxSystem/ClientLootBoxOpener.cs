using System.Collections.Generic;
using System.Linq;
using DataStructures;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class ClientLootBoxOpener : ILootBoxOpener
    {
        private IStorage<int, DeckCard> cardStorage;
        public LootBoxInfo BoxInfo {get; private set;}

        public ClientLootBoxOpener(LootBoxInfo info, IStorage<int, DeckCard> cardStorage)
        {
            BoxInfo = info;
            this.cardStorage = cardStorage;
        }

        public bool CanOpen(IReadOnlyUserInfo info)
        {
            return info.LootBoxes.ContainsKey(BoxInfo.BoxType)
                && info.LootBoxes[BoxInfo.BoxType] > 0;
        }

        public DropInfo Open(IReadOnlyUserInfo info)
        {
            var drops = new List<Drop>();
            foreach(var loot in BoxInfo.AllLoot)
            {
                switch(loot.LootType)
                {
                    case LootType.Gold:
                        drops.Add(HandleUsualDrop(
                            loot.LootType, 
                            loot.MinGoldChances, 
                            loot.MaxGoldChances));
                        break;
                    case LootType.Diamond:
                        drops.Add(HandleUsualDrop(
                            loot.LootType, 
                            loot.MinDiamondChances,
                            loot.MaxDiamondChances)); 
                        break;
                    case LootType.UpgradeCard:
                        drops.Add(HandleUsualDrop(
                            loot.LootType,
                            loot.MinUpgradeCardChances,
                            loot.MaxUpgradeCardChances));
                        break;
                    case LootType.Card:
                        drops.Add(HandleCard(loot));
                        break;
                }
            }

            return new(drops);
        }
        
        private Drop HandleUsualDrop(LootType lootType, int min, int max)
        {
            var value = Random.Range(min, max);
            var drop = new Drop(lootType, value);
            return drop;
        }

        private Drop HandleCard(LootInfo info)
        {
            var chanceList = new RandomChanceList<Rarity>();
            foreach (var cardChance in info.CardChances)
            {
                chanceList.Add(cardChance.Rarity, cardChance.Chance);
            }
            var rarity = chanceList.PickRandomObject();

            var elligbleCards = cardStorage.FindCardsByType(rarity).ToArray();
            var randomCard = Random.Range(0, elligbleCards.Length);
            return new Drop(LootType.Card, elligbleCards[randomCard]);
        }

        private IEnumerable<int> FindAllElligbleCards(Rarity rarity)
        {
            foreach(var card in cardStorage.Values)
            {
                if (card.Rarity == rarity)
                    yield return cardStorage.ValueToId[card];
            }
        }
    }
}
