using System;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class LootBoxesPanel : MonoBehaviour
    {
        [SerializeField] private LootBoxDrawer lootBoxDrawerPrefab;
        [SerializeField] private LootBoxDrawer buyPanelDrawer;
        [SerializeField] private LayoutGroup boxesLayout;
        [SerializeField] private BoxImagesShower boxImagesShower;

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
                instance.Redraw(box, () =>
                {
                    buyPanelDrawer.Button.interactable = GameRoot.Instance.LootBoxController.CanBuy(box.BoxType);
                    buyPanelDrawer.gameObject.SetActive(true); 
                    buyPanelDrawer.Redraw(box, () => OnButtonClick(box));
                });
            }
        }
        
        private void OnButtonClick(LootBoxInfo lootBoxInfo)
        {
            if (!GameRoot.Instance.LootBoxController.CanBuy(lootBoxInfo.BoxType))
                throw new InvalidOperationException("Can't buy this Box");
            GameRoot.Instance.LootBoxController.Buy(lootBoxInfo.BoxType);
            
            var dropInfo = GameRoot.Instance.LootBoxController.Open(lootBoxInfo.BoxType);
            boxImagesShower.gameObject.SetActive(true);
            boxImagesShower.ShowItems(dropInfo.Drops);
        }
    }
}
