using System;
using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public class LootBoxController : MonoBehaviour
    {
        [SerializeField] private LootBoxInfo[] infos;
        private Dictionary<LootBoxType, LootBoxInfo> typeToInfos = new();
        private Dictionary<LootBoxType, ILootBoxOpener> openers;
        private UserInfoController userInfoController;
        private IReadOnlyUserInfo userInfo;
        private IConverter<DropInfo, ContextedDropInfo> dropConverter;
        public IEnumerable<LootBoxInfo> lootBoxes => typeToInfos.Values;

        public void Initialize(
            IReadOnlyUserInfo userInfo,
            ILootBoxOpenerFabric openerFabric,
            IConverter<DropInfo, ContextedDropInfo> dropConverter,
            UserInfoController userInfoController)
        {
            this.userInfo = userInfo;
            openers = new();
            this.dropConverter = dropConverter;
            this.userInfoController = userInfoController;
            foreach (var boxInfo in infos)
            {
                if(typeToInfos.ContainsKey(boxInfo.BoxType))
                    throw new ArgumentException("Loot Box Infos should be unique by rarity!");
                typeToInfos[boxInfo.BoxType] = boxInfo;
                openers[boxInfo.BoxType] = openerFabric.Create(boxInfo);
            }
        }

        public bool CanOpen(LootBoxType boxType)
        {
            if (!openers.ContainsKey(boxType))
                return false;
            var opener = openers[boxType];
            return opener.CanOpen(userInfo);
        }

        public ContextedDropInfo Open(LootBoxType boxType)
        {
            var opener = openers[boxType];
            var dropInfo = opener.Open(userInfo);
            var currentBoxes = userInfoController.GetBoxes(boxType);
            userInfoController.SetBoxes(boxType, currentBoxes - 1);
            var result = dropConverter.Convert(dropInfo);
            SaveDrop(result);
            return result;
        }
        //Получатель: Артём
        //Отправитель: Артём
        //Прошу тебя разобраться с этим ужасом!
        private void SaveDrop(ContextedDropInfo dropInfo)
        {
            foreach(var drop in dropInfo.Drops)
            {
                switch(drop.Drop.DropType)
                {
                    case (LootType.Gold):
                        userInfoController.UserGold += drop.Drop.Value;
                        break;
                    case (LootType.UpgradeCard):
                        userInfoController.UserUpgradeCards += drop.Drop.Value;
                        break;
                    case (LootType.Diamond):
                        userInfoController.UserDiamond += drop.Drop.Value;
                        break;
                    case (LootType.Card):
                        userInfoController.UnlockCard(drop.Drop.Value);
                        break;
                    case (LootType.Blessing):
                        userInfoController.GlobalBlessingsPull[drop.Drop.BlessingId] += drop.Drop.Value;
                        break;
                }
            }
        }

        public bool CanBuy(LootBoxType boxType, int amount = 1)
        {
            var box = typeToInfos[boxType];
            switch (box.CostType)
            {
                case CostType.Gold:
                    return userInfoController.UserGold >= box.Cost * amount;
                case CostType.Diamond:
                    return userInfoController.UserDiamond >= box.Cost * amount;
                default:
                    throw new NotImplementedException();
            }
        }

        public void Buy(LootBoxType boxType, int amount)
        {
            if (amount < 0)
                throw new ArgumentException("You can't buy less than zero loot boxes!");
            var currentBoxes = userInfoController.GetBoxes(boxType);
            userInfoController.SetBoxes(boxType, currentBoxes + amount);
            var box = typeToInfos[boxType];

            switch(box.CostType)
            {
                case CostType.Gold:
                    userInfoController.UserGold -= box.Cost * amount;
                    break;
                case CostType.Diamond:
                    userInfoController.UserDiamond -= box.Cost * amount;
                    break;
            }

            
        }

        public void Buy(LootBoxType boxType) => Buy(boxType, 1);

    }
}
