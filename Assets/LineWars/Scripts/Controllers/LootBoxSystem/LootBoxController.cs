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
        private Dictionary<LootBoxType, LootBoxInfo> typeToInfos = new();
        private Dictionary<LootBoxType, ILootBoxOpener> openers;
        private UserInfoController userInfoController;
        private IProvider<UserInfo> provider;
        private IConverter<DropInfo, ContextedDropInfo> dropConverter;
        public IEnumerable<LootBoxInfo> lootBoxes => typeToInfos.Values;

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
            var userInfo = provider.Load(0);
            return opener.CanOpen(userInfo);
        }

        public ContextedDropInfo Open(LootBoxType boxType)
        {
            var opener = openers[boxType];
            var userInfo = provider.Load(0);
            var dropInfo = opener.Open(userInfo);
            var result = dropConverter.Convert(dropInfo);
            SaveDrop(result);
            return result;
        }
        //����������: ����
        //�����������: ����
        //����� ���� ����������� � ���� ������!
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
                }
            }
        }

        public bool CanBuy(LootBoxType boxType)
        {
            var box = typeToInfos[boxType];
            switch (box.CostType)
            {
                case CostType.Gold:
                    return userInfoController.UserGold >= box.Cost;
                case CostType.Diamond:
                    return userInfoController.UserDiamond >= box.Cost;
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
                    userInfoController.UserGold -= box.Cost;
                    break;
                case CostType.Diamond:
                    userInfoController.UserDiamond -= box.Cost;
                    break;
            }

            
        }

        public void Buy(LootBoxType boxType) => Buy(boxType, 1);

    }
}