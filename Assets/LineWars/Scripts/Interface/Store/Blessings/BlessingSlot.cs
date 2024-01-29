using System.Linq;
using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class BlessingSlot : MonoBehaviour, IDropHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text amount;
        [SerializeField] private Image background;
        [SerializeField] private Image questionImage;
        
        [SerializeField] private int index;
        public void ReDraw(BlessingReDrawInfo blessingReDrawInfo)
        {
            SetActive(blessingReDrawInfo != null);
            if(blessingReDrawInfo == null)
                return;
            Init(blessingReDrawInfo);
        }

        public void OnDrop(PointerEventData eventData)
        {
            var dragableItem = eventData.pointerDrag.GetComponent<DraggableBlessing>();
            var blessingId = dragableItem.BlessingId;

            var x = GameRoot.Instance.BlessingsController.SelectedBlessings.ToArray();
            var active = GameRoot.Instance.BlessingsController.SelectedBlessings.All(x => x != blessingId);
            SetActive(active);
            if(!active)
                return;
            Init(DrawHelper.GetBlessingReDrawInfoByBlessingId(blessingId));
            GameRoot.Instance.BlessingsController.SelectedBlessings[index] = blessingId;
        }

        private void Init(BlessingReDrawInfo blessingReDrawInfo)
        {
            image.sprite = blessingReDrawInfo.Sprite;
            amount.text = blessingReDrawInfo.Amount.ToString();
            background.color = blessingReDrawInfo.BgColor;
        }
        
        private void SetActive(bool active)
        {
            image.gameObject.SetActive(active);
            amount.gameObject.SetActive(active);
            background.color = active ? Color.white : new Color32(120, 120, 120, 255);
            questionImage.gameObject.SetActive(!active);
            
            if (active == false)
            {
                GameRoot.Instance.BlessingsController.SelectedBlessings[index] = BlessingId.Null;
            }
        }
    }
}
