using UnityEngine;

namespace LineWars.Controllers
{
    public class Selectable2D: MonoBehaviour
    {
        private void OnMouseDown()
        {
            Selector.SelectedObject = gameObject;
        }
    }
}