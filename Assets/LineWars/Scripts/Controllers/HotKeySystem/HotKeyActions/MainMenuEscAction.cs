using UnityEngine;

namespace LineWars.Controllers
{
    public class MainMenuEscAction: HotKeyAction
    {
        [SerializeField] private Transform settingsPanel;
        [SerializeField] private UIStack uiStack;
        public override void Invoke()
        {
            if (uiStack.PeekElement() == settingsPanel)
            {
                uiStack.PopElement();
            }
            else
            {
                uiStack.PushElement(settingsPanel);
            }
        }
    }
}