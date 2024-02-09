﻿using System;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using Utilities.Runtime;

namespace LineWars.Interface
{
    public class PaymentCatalog: MonoBehaviour
    {
        private static SDKAdapterBase SdkAdapter => GameRoot.Instance.SdkAdapter;
        
        [Header("Settings")]
        [SerializeField] private bool spawnPurchases = true;
        [ConditionallyVisible(nameof(spawnPurchases)), Tooltip("Родительский объект для спавна в нём покупок")]
        [SerializeField] private Transform rootSpawnPurchases;
        [ConditionallyVisible(nameof(spawnPurchases)), Tooltip("Префаб покупки (объект со компонентом PurchaseYG)")]
        [SerializeField] private GameObject purchasePrefab;
        
        [Tooltip("Когда следует обновлять список покупок?\nStart - Обновлять в методе Start.\nOnEnable - Обновлять при каждой активации объекта (в методе OnEnable)\nDoNotUpdate - Не обновлять.")]
        [SerializeField]private UpdateListMethod updateListMethod;

        [Tooltip("Список покупок")]
        [SerializeField]public PurchaseDrawer[] purchasesDrawers = Array.Empty<PurchaseDrawer>();

        [SerializeField] private PrizeType prizeType;
        
        public Action OnUpdatePurchasesList;
        private void OnEnable()
        {
            if (updateListMethod != UpdateListMethod.DoNotUpdate)
                SdkAdapter.PurchasesUpdated += UpdatePurchasesList;

            if (SdkAdapter.SDKEnabled && updateListMethod == UpdateListMethod.OnEnable)
                UpdatePurchasesList();
        }

        private void OnDisable()
        {
            if (updateListMethod != UpdateListMethod.DoNotUpdate)
                SdkAdapter.PurchasesUpdated -= UpdatePurchasesList;
        }

        private void Start()
        {
            if (updateListMethod == UpdateListMethod.Start)
            {
                UpdatePurchasesList();
            }
        }

        public void UpdatePurchasesList()
        {
            if (spawnPurchases)
            {
                DestroyPurchasesList();
                SpawnPurchasesList();
            }
            else
            {
                SetDataPurchasesListByID();
            }
            OnUpdatePurchasesList?.Invoke();
        }

        private void DestroyPurchasesList()
        {
            int childCount = rootSpawnPurchases.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                Destroy(rootSpawnPurchases.GetChild(i).gameObject);
            }
        }

        private void SpawnPurchasesList()
        {
            var purchases = SdkAdapter.GetPurchases(prizeType);
            
            purchasesDrawers = new PurchaseDrawer[purchases.Length];
            for (var i = 0; i < purchases.Length; i++)
            {
                var purchase = purchases[i]; 
                var purchaseObj = Instantiate(purchasePrefab, rootSpawnPurchases);

                purchasesDrawers[i] = purchaseObj.GetComponent<PurchaseDrawer>();
                purchasesDrawers[i].Data = purchase;
                purchasesDrawers[i].UpdateEntries();
            }
        }

        private void SetDataPurchasesListByID()
        {
            for (int i = 0; i < purchasesDrawers.Length; i++)
            {
                UserPurchaseInfo purchaseInfo = SdkAdapter.PurchaseByID(purchasesDrawers[i].Data.Id);
                if (purchaseInfo != null)
                {
                    purchasesDrawers[i].Data = purchaseInfo;
                }
                else
                {
                    Debug.LogError($"Purchase with ID: {purchasesDrawers[i].Data.Id} not found!");
                    continue;
                }

                purchasesDrawers[i].UpdateEntries();
            }
        }
    }

    public enum UpdateListMethod
    {
        OnEnable,
        Start, 
        DoNotUpdate
    };
}