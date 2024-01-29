using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Controllers;
using LineWars.LootBoxes;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class BlessingsBuyPanelDrawer : MonoBehaviour
    {
        [SerializeField] private BlessingBuyDrawer blessingBuyDrawerPrefab;
        [SerializeField] private BuyPanel buyPanel;
        [SerializeField] private LayoutGroup blessingsLayout;

        private void Awake()
        {
            ReDrawBoxes();
        }

        private void ReDrawBoxes()
        {
            var blessings = GameRoot.Instance.CardStore.BlessingsForPurchase;
            foreach (var blessing in blessings)
            {
                var instance = Instantiate(blessingBuyDrawerPrefab, blessingsLayout.transform);
                instance.ReDraw(blessing, () => buyPanel.OpenWindow(GetBuyPanelReDrawInfo(blessing)));
            }
        }
        
        private BuyPanelReDrawInfo GetBuyPanelReDrawInfo(BlessingId blessingId)
        {
            return new BuyPanelReDrawInfo(() => OnButtonClick(blessingId),
                () => GameRoot.Instance.CardStore.CanBuy(blessingId),
                DrawHelper.GetSpriteByBlessingID(blessingId),
                DrawHelper.GetBlessingNameByBlessingID(blessingId),
                DrawHelper.GetBlessingDescription(blessingId),
                GameRoot.Instance.CardStore.BlessingsCost[blessingId],
                CostType.Gold);
        }
        
        private void OnButtonClick(BlessingId blessingId)
        {
            if (!GameRoot.Instance.CardStore.CanBuy(blessingId))
                throw new InvalidOperationException("Can't buy this Blessing");
            GameRoot.Instance.CardStore.Buy(blessingId);
        }
    }
}
