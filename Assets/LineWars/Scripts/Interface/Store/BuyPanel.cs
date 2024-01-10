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

        [SerializeField] private TMP_Text costText;
        [SerializeField] private Image lootBoxImage;
        [SerializeField] private Image coinsImage;
        [SerializeField] private Image diamondsImage;
        [SerializeField] private TMP_Text boxName;
        [SerializeField] private TMP_Text boxDescription;

        private readonly Color coinsColor = new (251, 184, 13);
        private readonly Color diamondsColor = new (254, 57, 59);
        
        public void NewOpenWindow(BuyPanelReDrawInfo buyPanelReDrawInfo)
        {
            var canBuy = buyPanelReDrawInfo.ButtonInteractivityBool.Invoke();
            buyAndOpenButton.interactable = canBuy;
            inactiveCanvasGroup.alpha = canBuy ? 1f : 0.5f;
            buyAndOpenButton.onClick.RemoveAllListeners();
            buyAndOpenButton.onClick.AddListener(buyPanelReDrawInfo.OnButtonClickAction);

            costText.text = buyPanelReDrawInfo.Cost.ToString();
            costText.color = buyPanelReDrawInfo.CostType == CostType.Gold ? coinsColor : diamondsColor;
            coinsImage.gameObject.SetActive(buyPanelReDrawInfo.CostType == CostType.Gold);
            diamondsImage.gameObject.SetActive(buyPanelReDrawInfo.CostType == CostType.Diamond);
            boxName.text = buyPanelReDrawInfo.PanelName;
            if (boxDescription != null) 
                boxDescription.text = buyPanelReDrawInfo.PanelDescription;
            lootBoxImage.sprite = buyPanelReDrawInfo.UnitSprite;
            
            gameObject.SetActive(true);
        }
    }
}
