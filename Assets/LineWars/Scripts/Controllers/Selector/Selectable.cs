using UnityEngine;
using UnityEngine.EventSystems;

namespace LineWars.Controllers
{
    public class Selectable: MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Selector.SelectedObject = gameObject;
        }
    }
}