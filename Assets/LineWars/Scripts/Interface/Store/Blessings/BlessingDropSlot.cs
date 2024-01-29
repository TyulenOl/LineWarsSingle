using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LineWars.Interface
{
    public class BlessingDropSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
    {
        [SerializeField] private BlessingInfoDrawer blessingInfoDrawer;
        private int index;
        private IBlessingSelector Selector => GameRoot.Instance.BlessingsController;

        public void Initialize(int index)
        {
            this.index = index;
        }

        public void Redraw(AllBlessingReDrawInfo reDrawInfo)
        {
            //Debug.Log("Redraw");
            blessingInfoDrawer.Redraw(reDrawInfo);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData == null
                || eventData.pointerDrag == null
                || !eventData.pointerDrag.TryGetComponent(out DraggableBlessing draggableBlessing))
                return;
            //Debug.Log("OnDrop");
            var blessingId = draggableBlessing.BlessingId;
            if (Selector.CanSetValue(index, blessingId))
            {
                Selector[index] = blessingId;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //Debug.Log("OnClick");
            Selector[index] = BlessingId.Null;
        }
    }
}
