using LineWars.Model;
using UnityEngine;

namespace LineWars.Interface
{
    public class BlessingDraggableSet : MonoBehaviour
    {
        [SerializeField] private BlessingInfoDrawer blessingInfoDrawer;
        [SerializeField] private DraggableBlessing draggableBlessing;

        public void Init(BlessingId blessingId)
        {
            draggableBlessing.Init(blessingId);
            blessingInfoDrawer.Init(DrawHelper.GetBlessingReDrawInfoByBlessingId(blessingId));
        }
    }
}