using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class BlessingsInfoDrawersList: MonoBehaviour
    {
        [SerializeField] private BlessingInfoDrawer blessingInfoDrawerPrefab;
        [SerializeField] private LayoutGroup layoutGroup;
        private BlessingInfoDrawer[] blessingInfoDrawers;
        private Dictionary<BlessingId, BlessingInfoDrawer> idToDrawer;
        private IBlessingSelector Selector => GameRoot.Instance.BlessingsController.SelectedBlessings;
        private IBlessingsPull BlessingsPull => GameRoot.Instance.UserController;

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
            
            CreateDrawers();
            Selector.SelectedBlessingIdChanged += OnSelectedBlessingIdChanged;
            Selector.TotalSelectionCountChanged += OnTotalSelectionCountChanged;
            BlessingsPull.BlessingCountChanged += BlessingsPullOnBlessingCountChanged;
        }

        private void BlessingsPullOnBlessingCountChanged(BlessingId id, int count)
        {
            if (idToDrawer.TryGetValue(id, out var drawer))
            {
                drawer.Redraw(DrawHelper.GetBlessingReDrawInfoByBlessingId(id));
            }
        }

        private void OnDestroy()
        {
            Selector.SelectedBlessingIdChanged -= OnSelectedBlessingIdChanged;
            Selector.TotalSelectionCountChanged -= OnTotalSelectionCountChanged;
            BlessingsPull.BlessingCountChanged -= BlessingsPullOnBlessingCountChanged;
        }

        private void DestroyDrawers()
        {
            foreach (var drawer in blessingInfoDrawers)
                Destroy(drawer.gameObject);
        }
        private void CreateDrawers()
        {
            idToDrawer = new Dictionary<BlessingId, BlessingInfoDrawer>(Selector.Count);
            blessingInfoDrawers = new BlessingInfoDrawer[Selector.Count];
            for (var i = Selector.Count - 1; i >= 0; i--)
            {
                var id = Selector[i];
                var fullInfo = DrawHelper.GetBlessingReDrawInfoByBlessingId(id);
                var instance = Instantiate(blessingInfoDrawerPrefab, layoutGroup.transform);
                instance.transform.SetAsFirstSibling();
                instance.Redraw(fullInfo);
                blessingInfoDrawers[i] = instance;
                idToDrawer[id] = instance;
            }
        }

        private void OnTotalSelectionCountChanged(int count)
        {
            DestroyDrawers();
            CreateDrawers();
        }

        private void OnSelectedBlessingIdChanged(BlessingId id, int index)
        {
            var allInfo = DrawHelper.GetBlessingReDrawInfoByBlessingId(id);
            blessingInfoDrawers[index].Redraw(allInfo);
        }
    }
}