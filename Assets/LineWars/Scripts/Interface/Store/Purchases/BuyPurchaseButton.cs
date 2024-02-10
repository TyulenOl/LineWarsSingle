using System;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class BuyPurchaseButton: MonoBehaviour
    {
        private static SDKAdapterBase SDKAdapter => GameRoot.Instance.SdkAdapter;
        
        [SerializeField] private Button button;
        [SerializeField] private PurchaseContainer purchaseContainer;
        
        private void OnEnable()
        {
            button.onClick.AddListener(BuyPurchase);
            if (purchaseContainer.Data != null)
                button.interactable = SDKAdapter.CanBuyPurchase(purchaseContainer.Data.Id);
        }
        
        private void OnDisable()
        {
            button.onClick.RemoveListener(BuyPurchase);
        }
        
        private void BuyPurchase()
        {
            if (purchaseContainer.Data != null)
                SDKAdapter.BuyPurchase(purchaseContainer.Data.Id);
        }
    }
}