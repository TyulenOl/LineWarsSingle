using LineWars.Controllers;
using LineWars.Infrastructure;
using UnityEngine;

namespace LineWars
{
    public class ReselectButton: ButtonClickHandler
    {
        [SerializeField] private ChooseCommandsCanvasPreset commandsCanvasPreset;
        protected override void OnClick()
        {
            CommandsManager.Instance.ReselectExecutor();
            commandsCanvasPreset.gameObject.SetActive(false);
        }
    }
}