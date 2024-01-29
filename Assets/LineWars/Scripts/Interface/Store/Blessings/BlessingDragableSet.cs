using LineWars.Model;
using UnityEngine;

namespace LineWars.Interface
{
    public class BlessingDragableSet : MonoBehaviour
    {
        [SerializeField] private BlessingInfoDrawer blessingInfoDrawer;
        [SerializeField] private DragableBlessing dragableBlessing;

        public void Init(BlessingId blessingId)
        {
            dragableBlessing.Init(blessingId);
            blessingInfoDrawer.Init(DrawHelper.GetBlessingReDrawInfoByBlessingId(blessingId));
        }
    }
}