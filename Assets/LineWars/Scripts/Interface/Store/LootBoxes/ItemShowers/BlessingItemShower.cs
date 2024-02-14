using LineWars.Interface;
using UnityEngine;

namespace LineWars
{
    public class BlessingItemShower: LootedItemShower
    {
        [SerializeField] private BlessingInfoDrawer blessingInfoDrawer;

        public void ShowItem(FullBlessingReDrawInfo reDrawInfo)
        {
            blessingInfoDrawer.Redraw(reDrawInfo);
        }
    }
}