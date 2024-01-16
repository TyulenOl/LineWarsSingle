﻿using System.Collections.Generic;
using System.Linq;
using LineWars.Model;

namespace LineWars.LootBoxes
{
    public class DuplicateEreaserDropConverter
        : IConverter<DropInfo, ContextedDropInfo>
    {
        private int upgradeForUsual = 1; //ПЕРЕНЕСТИ В ЭДИТОР!!!!!! //ПЕРЕНЕСТИ В ЭДИТОР!!!!!!//ПЕРЕНЕСТИ В ЭДИТОР!!!!!!//ПЕРЕНЕСТИ В ЭДИТОР!!!!!!//ПЕРЕНЕСТИ В ЭДИТОР!!!!!!//ПЕРЕНЕСТИ В ЭДИТОР!!!!!!//ПЕРЕНЕСТИ В ЭДИТОР!!!!!!
        private int upgradeForLegendary = 2; // TODO: ПЕРЕНЕСТИ В ЭДИТОР!!!!!!
        private IDownloader<UserInfo> userInfoLoader;
        private IStorage<DeckCard> cardStorage;

        public DuplicateEreaserDropConverter(
            IDownloader<UserInfo> userInfoLoader,
            IStorage<DeckCard> cardStorage)
        {
            this.userInfoLoader = userInfoLoader;
            this.cardStorage = cardStorage;
        }

        public ContextedDropInfo Convert(DropInfo dropInfo)
        {
            var contextedDrops = new List<ContextedDrop>();
            var userInfo = userInfoLoader.Load(0);
            foreach(var drop in dropInfo.Drops)
            {
                if (drop.DropType != LootType.Card ||
                    !userInfo.UnlockedCards.Contains(drop.Value))
                {
                    var newDrop = new ContextedDrop(drop);
                    contextedDrops.Add(newDrop);
                    continue;
                }

                var card = cardStorage.IdToValue[drop.Value];
                var addedUpgrades = upgradeForUsual;
                if (card.Rarity == CardRarity.Legendary)
                    addedUpgrades = upgradeForLegendary;

                var revisedDrop = new Drop(LootType.UpgradeCard, addedUpgrades);
                var newDrop1 = new ContextedDrop(revisedDrop,drop);
                contextedDrops.Add(newDrop1);  
            }

            return new ContextedDropInfo(contextedDrops);
        }
    }
}
