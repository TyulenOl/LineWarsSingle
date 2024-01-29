using System;
using System.Collections;
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
        [SerializeField] private List<BlessingSlot> blessingSlots;
        [SerializeField] private BlessingsGroupDrawer blessingsGroupDrawerPrefab;
        [SerializeField] private LayoutGroup blessingsLayoutGroup;

        private Dictionary<BlessingType, BlessingsGroupDrawer> groupDrawers;

        private void Awake()
        {
            groupDrawers = new Dictionary<BlessingType, BlessingsGroupDrawer>();
            Init();
        }

        private void OnEnable()
        {
            for (var i = 0; i < blessingSlots.Count; i++)
            {
                var blessingReDraw =
                    DrawHelper.GetBlessingReDrawInfoByBlessingId(
                        GameRoot.Instance.BlessingsController.SelectedBlessings[i]);
                if(i >= GameRoot.Instance.BlessingsController.SelectedBlessings.Count)
                    return;
                var slot = blessingSlots[i];
                slot.ReDraw(blessingReDraw);
            }
        }

        private void Init()
        {
            foreach (var grouping in GameRoot.Instance.UserController.GlobalBlessingsPull.GroupBy(x => x.Item1.BlessingType, y => y.Item1))
            {
                var group = Instantiate(blessingsGroupDrawerPrefab, blessingsLayoutGroup.transform);
                group.Init(grouping);
                groupDrawers[grouping.Key] = group;
            }
        }
    }
}
