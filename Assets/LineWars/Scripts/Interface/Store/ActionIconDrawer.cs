using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class ActionIconDrawer: MonoBehaviour
    {
        [SerializeField] private Image icon;

        public void Redraw(CommandType commandType)
        {
            var sprite = DrawHelper.GetIconByCommandType(commandType);
            if (sprite)
                icon.sprite = sprite;
            else
                gameObject.SetActive(false);
        }
    }
}