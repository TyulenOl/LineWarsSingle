using System;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

namespace LineWars.LootBoxes
{
    public class LootBoxController : MonoBehaviour
    {
        [SerializeField] private LootBoxInfo[] infos;
        private Dictionary<LootBoxRarity, LootBoxOpener> openers;
        private UserInfoController userInfoController;
        private IProvider<UserInfo> provider;
        private IConverter<DropInfo, ContextedDropInfo> dropConverter;

        public void Initialize(
            IProvider<UserInfo> provider,
            ILootBoxOpenerFabric openerFabric,
            IConverter<DropInfo, ContextedDropInfo> dropConverter,
            UserInfoController userInfoController)
        {
            this.provider = provider;
            openers = new();
            this.dropConverter = dropConverter;
            this.userInfoController = userInfoController;
            foreach (var boxInfo in infos)
            {
                if (openers.ContainsKey(boxInfo.Rarity))
                    throw new ArgumentException("Loot Box Infos should be unique by rarity!");
                openers[boxInfo.Rarity] = openerFabric.Create(boxInfo);
            }
        }

        public bool CanOpen(LootBoxRarity boxRarity)
        {
            if (!openers.ContainsKey(boxRarity))
                return false;
            var opener = openers[boxRarity];
            var userInfo = provider.Load(0);
            return opener.CanOpen(userInfo);
        }

        public ContextedDropInfo Open(LootBoxRarity boxRarity)
        {
            var opener = openers[boxRarity];
            var userInfo = provider.Load(0);
            var dropInfo = opener.Open(userInfo);
            var result = dropConverter.Convert(dropInfo);
            SaveDrop(result);
            return result;
        }
        //Получатель: Артём
        //Отправитель: Артём
        //Прошу тебя разобраться с этим ужасом!
        public void SaveDrop(ContextedDropInfo dropInfo)
        {
            foreach(var drop in dropInfo.Drops)
            {
                switch(drop.Drop.DropType)
                {
                    case (LootType.Gold):
                        userInfoController.ChangeGold(drop.Drop.Value);
                        break;
                    case (LootType.UpgradeCard):
                        userInfoController.ChangeUpgradeCards(drop.Drop.Value);
                        break;
                    case (LootType.Diamond):
                        userInfoController.ChangeDiamond(drop.Drop.Value);
                        break;
                    case (LootType.Card):
                        userInfoController.UnlockCard(drop.Drop.Value);
                        break;
                }
            }
        }
    }
}
