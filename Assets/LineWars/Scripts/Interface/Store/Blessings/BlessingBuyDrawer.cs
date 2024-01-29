using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.LootBoxes;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LineWars
{
    public class BlessingBuyDrawer : MonoBehaviour
    {
        [SerializeField] private Image blessingImage;
        [SerializeField] private CostDrawer costDrawer;
        [SerializeField] private TMP_Text blessingName;
        [SerializeField] private Button buyButton;
        
        
        public void ReDraw(BlessingId blessingId, UnityAction action)
        {
            var cost = 9999999;
            if (GameRoot.Instance.CardStore.BlessingsCost.TryGetValue(blessingId, out var costValue))
            {
                cost = GameRoot.Instance.CardStore.BlessingsCost[blessingId];
            }
            costDrawer.DrawCost(cost, CostType.Gold);
            blessingImage.sprite = DrawHelper.GetSpriteByBlessingID(blessingId);
            blessingName.text = DrawHelper.GetBlessingNameByBlessingID(blessingId);
            buyButton.onClick.AddListener(action);
        }
    }
}
