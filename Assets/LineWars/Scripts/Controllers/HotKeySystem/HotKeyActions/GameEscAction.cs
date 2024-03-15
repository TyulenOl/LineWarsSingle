using UnityEngine;

namespace LineWars.Controllers
{
    public class GameEscAction: HotKeyAction
    {
        [SerializeField] private UIStack uiStack;
        [SerializeField] private Transform menuPanel;
        [SerializeField] private Transform settingsPanel;
        [SerializeField] private Transform gamePanel;
        
        public override void Invoke()
        {
            if (uiStack.PeekElement() == menuPanel
                || uiStack.PeekElement() == settingsPanel)
            {
                while (uiStack.PeekElement() != gamePanel)
                {
                    uiStack.PopElement();
                }
                SingleGameRoot.Instance.ResumeGame();
            }
            else
            {
                uiStack.PushElement(menuPanel);
                SingleGameRoot.Instance.PauseGame();
            }
        }
    }
}