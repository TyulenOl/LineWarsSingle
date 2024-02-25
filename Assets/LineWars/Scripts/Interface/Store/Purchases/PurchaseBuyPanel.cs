using System;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Interface
{
    public class PurchaseBuyPanel: MonoBehaviour
    {
        [SerializeField] private PurchaseDrawer purchaseDrawer;

        public void OpenWindow(ProductData purchaseInfo)
        {
            gameObject.SetActive(true);
            
            purchaseDrawer.Data = purchaseInfo;
            purchaseDrawer.UpdateEntries();
        }
    }
}