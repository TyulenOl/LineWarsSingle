using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars
{
    public class ChooseBlessingsPanel : MonoBehaviour
    {
        [SerializeField] private List<BlessingSlot> blessingSlots;

        private void Awake()
        {
            for (var i = 0; i < GameRoot.Instance.BlessingsController.CurrentBlessings.Count; i++)
            {
                if(i == blessingSlots.Count)
                    return;
                var slot = blessingSlots[i];
                slot.ReDraw(DrawHelper.GetBlessingReDrawInfoByBlessingId(GameRoot.Instance.BlessingsController.CurrentBlessings[i]));
            }
        }
    }
}
