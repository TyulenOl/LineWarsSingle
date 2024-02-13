using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class ChooseBlessingsPanel : MonoBehaviour
    {
        [SerializeField] private BlessingDropSlot blessingDropSlotPrefab;
        [SerializeField] private BlessingsGroupDrawer blessingsGroupDrawerPrefab;
        [SerializeField] private LayoutGroup blessingsGroupsLayoutGroup;
        [SerializeField] private LayoutGroup blessingSlotsLayoutGroup;
        private IBlessingSelector Selector => GameRoot.Instance.BlessingsController;
        private IBlessingsPull GlobalBlessingPull => GameRoot.Instance.UserController;
        private IStorage<BlessingId, BaseBlessing> AllBlessings => GameRoot.Instance.BlessingStorage;
        
        private Dictionary<BlessingType, BlessingsGroupDrawer> typeToDrawers;
        private BlessingDropSlot[] blessingSlots;
        private Dictionary<BlessingId, BlessingDropSlot> idToSlot;

        private bool initialized;

        private void Start()
        { 
            Initialize();
        }
        
        public void Initialize()
        {
            if (initialized)
                return;
            initialized = true;
            
            CreateSlotsList();
            CreateAllGroups();
            
            Selector.SelectedBlessingIdChanged += SelectorOnSelectedBlessingIdChanged;
            Selector.TotalSelectionCountChanged += SelectorOnTotalSelectionCountChanged;
            
            GlobalBlessingPull.BlessingCountChanged += OnBlessingCountChanged;
        }
        
        private void OnDestroy()
        {
            if (Selector != null)
            {
                Selector.SelectedBlessingIdChanged -= SelectorOnSelectedBlessingIdChanged;
                Selector.TotalSelectionCountChanged -= SelectorOnTotalSelectionCountChanged;
                
                GlobalBlessingPull.BlessingCountChanged -= OnBlessingCountChanged;
            }
        }
        
        private void OnBlessingCountChanged(BlessingId id, int count)
        {
            var allInfo = DrawHelper.GetBlessingReDrawInfoByBlessingId(id);
            typeToDrawers[id.BlessingType].RarityToSet[id.Rarity].Redraw(id);

            if (idToSlot.TryGetValue(id, out var slot))
                slot.Redraw(allInfo);
        }

        private void SelectorOnTotalSelectionCountChanged(int newCount)
        {
            DestroyAllSlots();
            CreateSlotsList();
        }

        private void SelectorOnSelectedBlessingIdChanged(BlessingId id, int index)
        {
            var info = DrawHelper.GetBlessingReDrawInfoByBlessingId(id);
            blessingSlots[index].Redraw(info);
        }

        private void CreateAllGroups()
        {
            typeToDrawers = new Dictionary<BlessingType, BlessingsGroupDrawer>();
            
            foreach (var grouping in AllBlessings.Keys.GroupBy(x => x.BlessingType))
            {
                var group = Instantiate(blessingsGroupDrawerPrefab, blessingsGroupsLayoutGroup.transform);
                typeToDrawers[grouping.Key] = group;
                group.Initialize(grouping.Key);
                
                foreach (var id in grouping.OrderBy(x => x.Rarity))
                    group.AddRarity(id.Rarity);
            }
            
            // foreach (var grouping in GlobalBlessingPull.Select(x => x.Item1).GroupBy(x => x.BlessingType))
            // {
            //     var group = Instantiate(blessingsGroupDrawerPrefab, blessingsGroupsLayoutGroup.transform);
            //     typeToDrawers[grouping.Key] = group;
            //     group.Initialize(grouping.Key);
            //     foreach (var id in grouping)
            //         group.AddRarity(id.Rarity);
            // }
        }

        private void DestroyAllSlots()
        {
            foreach (var slot in blessingSlots)
                Destroy(slot.gameObject);
        }

        private void CreateSlotsList()
        {
            idToSlot = new Dictionary<BlessingId, BlessingDropSlot>(Selector.Count);
            blessingSlots = new BlessingDropSlot[Selector.Count];
            for (var i = 0; i < Selector.Count; i++)
            {
                var info = DrawHelper.GetBlessingReDrawInfoByBlessingId(Selector[i]);
                var instance = Instantiate(blessingDropSlotPrefab, blessingSlotsLayoutGroup.transform);
                instance.Initialize(i);
                blessingSlots[i] = instance;
                instance.Redraw(info);
                idToSlot[Selector[i]] = instance;
            }
        }
    }
}
