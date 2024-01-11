using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.LootBoxes;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class LootBoxesPanel : MonoBehaviour
    {
        [SerializeField] private LootBoxDrawer lootBoxDrawerPrefab;
        [SerializeField] private BuyPanel buyPanel;
        [SerializeField] private LayoutGroup boxesLayout;
        [SerializeField] private BoxImagesShower boxImagesShower;

        private void Awake()
        {
            ReDrawBoxes();
        }

        private void ReDrawBoxes()
        {
            var boxes = GameRoot.Instance.LootBoxController.lootBoxes;
             foreach (var box in boxes)
             {
                 var instance = Instantiate(lootBoxDrawerPrefab, boxesLayout.transform);
                 instance.Init(box, () => buyPanel.OpenWindow(GetBuyPanelReDrawInfo(instance.LootBoxInfo)));
             }
        }
        
        private BuyPanelReDrawInfo GetBuyPanelReDrawInfo(LootBoxInfo lootBoxInfo)
        {
            return new BuyPanelReDrawInfo(() => OnButtonClick(lootBoxInfo),
                () => GameRoot.Instance.LootBoxController.CanBuy(lootBoxInfo.BoxType),
                lootBoxInfo.BoxSprite,
                lootBoxInfo.Name,
                lootBoxInfo.Description,
                lootBoxInfo.Cost,
                lootBoxInfo.CostType);
        }
        
        private void OnButtonClick(LootBoxInfo lootBoxInfo)
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
            boxImagesShower.gameObject.SetActive(true);
            boxImagesShower.ShowItems(dropInfo.Drops);
        }
    }
}
