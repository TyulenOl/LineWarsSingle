using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.LootBoxes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class BuyBoxPanel : MonoBehaviour
    {
        [SerializeField] private Button buyAndOpenButton;
        [SerializeField] private CanvasGroup inactiveCanvasGroup;
        [SerializeField] private LootBoxDrawer lootBoxDrawer;
        [SerializeField] private BoxImagesShower boxImagesShower;
        private LootBoxInfo lootBoxInfo;
        
        public void OpenWindow(LootBoxInfo lootBoxInfo)
        {
            var canBuy = GameRoot.Instance.LootBoxController.CanBuy(lootBoxInfo.BoxType);
            buyAndOpenButton.interactable = canBuy;
            inactiveCanvasGroup.alpha = canBuy ? 1f : 0.5f;
            buyAndOpenButton.onClick.AddListener(OnButtonClick);
            lootBoxDrawer.Init(lootBoxInfo);
            this.lootBoxInfo = lootBoxInfo;
            gameObject.SetActive(true);
        }

        private void OnButtonClick()
        {
            if (!GameRoot.Instance.LootBoxController.CanBuy(lootBoxInfo.BoxType))
                throw new InvalidOperationException("Can't buy this Box");
            GameRoot.Instance.LootBoxController.Buy(lootBoxInfo.BoxType);
            if (!GameRoot.Instance.LootBoxController.CanOpen(lootBoxInfo.BoxType))
            {
                if (lootBoxInfo.CostType == CostType.Gold)
                    GameRoot.Instance.UserController.UserGold += lootBoxInfo.Cost;
                else
                    GameRoot.Instance.UserController.UserDiamond += lootBoxInfo.Cost;
                throw new InvalidOperationException("Can't open this Box, your money didnt spent");
            }

            var dropInfo = GameRoot.Instance.LootBoxController.Open(lootBoxInfo.BoxType);
            foreach (var drop in dropInfo.Drops)    
            {
                Debug.Log(drop.Drop.DropType);
            }
            boxImagesShower.gameObject.SetActive(true);
            //boxImagesShower.ShowItems(dropInfo.Drops);
        }
    }
}
