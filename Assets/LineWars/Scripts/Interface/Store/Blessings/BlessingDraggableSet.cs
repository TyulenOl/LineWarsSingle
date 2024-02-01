using LineWars.Model;
using UnityEngine;

namespace LineWars.Interface
{
    public class BlessingDraggableSet : MonoBehaviour
    {
        [SerializeField] private BlessingInfoDrawer blessingInfoDrawer;
        [SerializeField] private DraggableBlessing draggableBlessing;

        public void Redraw(BlessingId blessingId)
        {
            draggableBlessing.Init(blessingId);
            blessingInfoDrawer.Redraw(DrawHelper.GetBlessingReDrawInfoByBlessingId(blessingId));
        }
    }
}