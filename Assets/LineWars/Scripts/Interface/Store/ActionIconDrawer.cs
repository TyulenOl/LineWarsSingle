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
            icon.sprite = DrawHelper.GetSpriteByCommandType(commandType);
        }
    }
}