using System.Collections;
using System.Collections.Generic;
using LineWars.Interface;
using UnityEngine;

namespace LineWars
{
    public class BlessingSlot : MonoBehaviour
    {
        [SerializeField] private BlessingInfoDrawer blessingDrawer;
        public void ReDraw(BlessingReDrawInfo blessingReDrawInfo)
        {
            blessingDrawer.Init(blessingReDrawInfo);
        }
    }
}
