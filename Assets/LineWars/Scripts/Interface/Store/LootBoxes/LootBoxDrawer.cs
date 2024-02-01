using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class LootBoxDrawer : MonoBehaviour
    {
        [SerializeField] private Image lootBoxImage;
        [SerializeField] private Image bgImage;
        [SerializeField] private TMP_Text boxName;
        [SerializeField] private TMP_Text boxDescription;
        [SerializeField] private Button button;
        [SerializeField] private CostDrawer costDrawer;
        
        [SerializeField] private LayoutGroup dropLayoutGroup;
        [SerializeField] private DropElement dropElementPrefab;

        private readonly List<DropElement> dropElements = new();

        private LootBoxInfo lootBoxInfo;
        public LootBoxInfo LootBoxInfo => lootBoxInfo;

        public Button Button => button;

        public void Redraw(LootBoxInfo lootBoxInfo, UnityAction onButtonClickAction = null)
        {
            button.onClick.RemoveAllListeners();
            
            costDrawer.DrawCost(lootBoxInfo.Cost, lootBoxInfo.CostType);
            boxName.text = lootBoxInfo.Name;
            if (boxDescription != null) 
                boxDescription.text = lootBoxInfo.Description;
            lootBoxImage.sprite = lootBoxInfo.BoxSprite;
            if(bgImage != null)
                bgImage.sprite = lootBoxInfo.BgSprite;
            if(onButtonClickAction != null)
                button.onClick.AddListener(onButtonClickAction);
            this.lootBoxInfo = lootBoxInfo;

            if (dropLayoutGroup == null || dropElementPrefab == null)
                return;
            
            foreach (var loot in lootBoxInfo.AllLoot
                         .Distinct(new LootTypeComparer())
                         .OrderBy(x => x.LootType))
            {
                DrawLoot(loot);
            }
        }

        private void DrawLoot(LootInfo lootInfo)
        {
            if (lootInfo.LootType == LootType.Card)
            {
                foreach (var card in lootInfo.CardChances.OrderBy(x => x.Rarity))
                {
                    var instance = Instantiate(dropElementPrefab, dropLayoutGroup.transform);
                    instance.Icon.sprite = DrawHelper.LootTypeToSprite[lootInfo.LootType];
                    instance.Background.color = DrawHelper.RarityToColor[card.Rarity];
                    dropElements.Add(instance);
                }
            }
            else
            {
                var instance = Instantiate(dropElementPrefab, dropLayoutGroup.transform);
                instance.Icon.sprite = DrawHelper.LootTypeToSprite[lootInfo.LootType];
                instance.Background.color = DrawHelper.RarityToColor[Rarity.Common];
                dropElements.Add(instance);
            }
        }

        private void OnDisable()
        {
            if (dropElements.Count > 0)
            {
                foreach (var dropElement in dropElements)
                    Destroy(dropElement.gameObject);
                dropElements.Clear();
            }
        }

        private class LootTypeComparer : IEqualityComparer<LootInfo>
        {
            public bool Equals(LootInfo x, LootInfo y)
            {
                return x != null
                       && y != null
                       && x.LootType == y.LootType;
            }

            public int GetHashCode(LootInfo obj)
            {
                return obj.LootType.GetHashCode();
            }
        }
    }
}
