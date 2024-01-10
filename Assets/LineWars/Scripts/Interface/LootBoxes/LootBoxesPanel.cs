using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.LootBoxes;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class LootBoxesPanel : MonoBehaviour
    {
        [SerializeField] private LootBoxDrawer lootBoxDrawerPrefab;
        [SerializeField] private BuyBoxPanel buyBoxPanel;
        [SerializeField] private LayoutGroup boxesLayout;

        private void Awake()
        {
            ReDrawBoxes();
        }

        private void ReDrawBoxes()
        {
            var boxes = GameRoot.Instance.LootBoxController.lootBoxes;
            foreach (var box in boxes)
            {
                var instance = Instantiate(lootBoxDrawerPrefab, boxesLayout.transform);
                instance.Init(box, () => buyBoxPanel.OpenWindow(box));
            }
        }
    }
}
