using System;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class BlessingsBuyPanelDrawer : MonoBehaviour
    {
        [SerializeField] private BlessingBuyDrawer blessingBuyDrawerPrefab;
        [SerializeField] private BuyPanel buyPanel;
        [SerializeField] private LayoutGroup blessingsLayout;

        private BlessingId selectedBlessing;
        private static Store Store => GameRoot.Instance.Store;

        private void Start()
        {
            ReDrawBoxes();
            buyPanel.OnClick.AddListener(BuyBlessing);
        }

        private void OnDestroy()
        {
            if (buyPanel != null)
                buyPanel.OnClick.RemoveListener(BuyBlessing);
        }


        private void ReDrawBoxes()
        {
            var blessings = GameRoot.Instance.Store.BlessingsForPurchase;
            foreach (var blessing in blessings)
            {
                var instance = Instantiate(blessingBuyDrawerPrefab, blessingsLayout.transform);
                instance.ReDraw(blessing);
                instance.OnClick.AddListener(() =>
                {
                    buyPanel.OpenWindow(GetBuyPanelReDrawInfo(blessing));
                    selectedBlessing = blessing;
                });
            }
        }
        
        private BuyPanelReDrawInfo GetBuyPanelReDrawInfo(BlessingId blessingId)
        {
            return new BuyPanelReDrawInfo(
                DrawHelper.GetSpriteByBlessingID(blessingId),
                DrawHelper.GetBlessingNameByBlessingID(blessingId),
                DrawHelper.GetBlessingDescription(blessingId),
                GameRoot.Instance.Store.BlessingToCost[blessingId],
                CostType.Gold,
                Store.CanBuy(blessingId));
        }
        
        private void BuyBlessing()
        {
            if (!Store.CanBuy(selectedBlessing))
                throw new InvalidOperationException("Can't buy this Blessing");
            Store.Buy(selectedBlessing);
        }
    }
}
