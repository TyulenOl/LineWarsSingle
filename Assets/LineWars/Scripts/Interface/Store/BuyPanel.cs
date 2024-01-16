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
    public class BuyPanel : MonoBehaviour
    {
        [SerializeField] private Button buyAndOpenButton;
        [SerializeField] private CanvasGroup inactiveCanvasGroup;
        
        [SerializeField] private Image lootBoxImage;
        [SerializeField] private TMP_Text boxName;
        [SerializeField] private TMP_Text boxDescription;
        [SerializeField] private CostDrawer costDrawer;

        private readonly Color coinsColor = new (251, 184, 13);
        private readonly Color diamondsColor = new (254, 57, 59);
        
        public void OpenWindow(BuyPanelReDrawInfo buyPanelReDrawInfo)
        {
            var canBuy = buyPanelReDrawInfo.ButtonInteractivityBool.Invoke();
            buyAndOpenButton.interactable = canBuy;
            inactiveCanvasGroup.alpha = canBuy ? 1f : 0.5f;
            buyAndOpenButton.onClick.RemoveAllListeners();
            buyAndOpenButton.onClick.AddListener(buyPanelReDrawInfo.OnButtonClickAction);

            costDrawer.DrawCost(buyPanelReDrawInfo.Cost, buyPanelReDrawInfo.CostType);
            boxName.text = buyPanelReDrawInfo.PanelName;
            if (boxDescription != null) 
                boxDescription.text = buyPanelReDrawInfo.PanelDescription;
            lootBoxImage.sprite = buyPanelReDrawInfo.UnitSprite;
            
            gameObject.SetActive(true);
        }
    }
}
