using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures;
using LineWars.Controllers;
using Random = UnityEngine.Random;

namespace LineWars.Model
{
    public class ClientLootBoxOpener : ILootBoxOpener
    {
        private readonly IStorage<int, DeckCard> cardStorage;
        private readonly IStorage<BlessingId, BaseBlessing> blessingStorage;
        public LootBoxInfo BoxInfo {get; private set;}

        public ClientLootBoxOpener(
            LootBoxInfo info,
            IStorage<int, DeckCard> cardStorage,
            IStorage<BlessingId, BaseBlessing> blessingStorage)
        {
            BoxInfo = info;
            this.cardStorage = cardStorage;
            this.blessingStorage = blessingStorage;
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
                    case LootType.Blessing:
                        drops.Add(HandleBlessing(loot));
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
        
        private Drop HandleBlessing(LootInfo info)
        {
            var chanceList = new RandomChanceList<Rarity>();
            foreach (var blessingChances in info.BlessingChances)
            {
                chanceList.Add(blessingChances.Rarity, blessingChances.Chance);
            }
            var rarity = chanceList.PickRandomObject();

            var blessingIds = blessingStorage.FindBlessingByType(rarity).ToArray();
            var randomCard = Random.Range(0, blessingIds.Length);

            throw new NotImplementedException();
            //return new Drop(LootType.Card);
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
