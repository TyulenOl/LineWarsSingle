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
        [SerializeField] private GameObject bg;
        [SerializeField] private TMP_Text boxName;
        [SerializeField] private TMP_Text boxDescription;
        [SerializeField] private Button button;
        [SerializeField] private CostDrawer costDrawer;
        [SerializeField] private Transform transformToGenerateSibling;
        
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
            
            RedrawBg(lootBoxInfo.Bg);
            
            if(onButtonClickAction != null)
                button.onClick.AddListener(onButtonClickAction);
            this.lootBoxInfo = lootBoxInfo;

            if (dropLayoutGroup == null || dropElementPrefab == null)
                return;
            
            foreach (var lootGroup in lootBoxInfo.AllLoot
                         .OrderBy(x => x.LootType)
                         .GroupBy(x => x.LootType))
            {
                DrawLoot(lootGroup);
            }
        }

        private void DrawLoot(IGrouping<LootType, LootInfo> LootGroup)
        {
            if (LootGroup.Key == LootType.Card)
            {
                var allRarities = LootGroup
                    .SelectMany(info => info.CardChances
                        .Select(chances => chances.Rarity))
                    .Distinct()
                    .OrderBy(rarity => rarity)
                    .ToArray();
                
                foreach (var rarity in allRarities)
                {
                    var instance = Instantiate(dropElementPrefab, dropLayoutGroup.transform);
                    instance.Icon.sprite = DrawHelper.LootTypeToSprite[LootGroup.Key];
                    instance.Background.color = DrawHelper.RarityToColor[rarity];
                    dropElements.Add(instance);
                }
            }
            else if (LootGroup.Key == LootType.Blessing)
            {
                var allRarities = LootGroup
                    .SelectMany(info => info.BlessingChances
                        .Select(chances => chances.Rarity))
                    .Distinct()
                    .OrderBy(rarity => rarity)
                    .ToArray();
                
                foreach (var rarity in allRarities)
                {
                    var instance = Instantiate(dropElementPrefab, dropLayoutGroup.transform);
                    instance.Icon.sprite = DrawHelper.LootTypeToSprite[LootGroup.Key];
                    instance.Background.color = DrawHelper.RarityToColor[rarity];
                    dropElements.Add(instance);
                }
            }
            else
            {
                var instance = Instantiate(dropElementPrefab, dropLayoutGroup.transform);
                instance.Icon.sprite = DrawHelper.LootTypeToSprite[LootGroup.Key];
                instance.Background.color = DrawHelper.LootTypeToColor[LootGroup.Key];
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
        
        private void RedrawBg(GameObject newBg)
        {
            if (bg != null)
                Destroy(bg.gameObject);
            var newBgInstance = Instantiate(newBg, transformToGenerateSibling);
            newBgInstance.transform.SetAsFirstSibling();
            bg = newBgInstance;
        }
    }
}
