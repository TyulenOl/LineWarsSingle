using System;
using LineWars.Model;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars
{
    public class BuyPanelReDrawInfo
    {
        public int Cost { get; }
        public CostType CostType { get; }
        public Sprite UnitSprite { get; }
        public string PanelName { get; }
        public string PanelDescription { get; }
        public bool ButtonInteractable { get; }

        public BuyPanelReDrawInfo(
            Sprite unitSprite,
            string panelName,
            string panelDescription,
            int cost,
            CostType costType, 
            bool buttonInteractable)
        {
            UnitSprite = unitSprite;
            PanelName = panelName;
            PanelDescription = panelDescription;
            Cost = cost;
            CostType = costType;
            ButtonInteractable = buttonInteractable;
        }
    }
}