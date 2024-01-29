using System;
using LineWars.Model;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars
{
    public class BuyPanelReDrawInfo
    {
        private UnityAction onButtonClickAction;
        private Func<bool> buttonInteractivityBool;
        private Sprite unitSprite;
        private string panelName;
        private string panelDescription;
        private int cost;
        private CostType costType;

        public int Cost => cost;

        public CostType CostType => costType;

        public UnityAction OnButtonClickAction => onButtonClickAction;

        public Func<bool> ButtonInteractivityBool => buttonInteractivityBool;

        public Sprite UnitSprite => unitSprite;

        public string PanelName => panelName;

        public string PanelDescription => panelDescription;

        public BuyPanelReDrawInfo(UnityAction onButtonClickAction,
            Func<bool> buttonInteractivityBool,
            Sprite unitSprite,
            string panelName,
            string panelDescription,
            int cost,
            CostType costType)
        {
            this.onButtonClickAction = onButtonClickAction;
            this.buttonInteractivityBool = buttonInteractivityBool;
            this.unitSprite = unitSprite;
            this.panelName = panelName;
            this.panelDescription = panelDescription;
            this.cost = cost;
            this.costType = costType;
        }
    }
}