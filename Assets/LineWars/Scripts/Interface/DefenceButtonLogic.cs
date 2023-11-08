using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class DefenceButtonLogic : MonoBehaviour
    {
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            var executor = CommandsManager.Instance.Executor;
            if (executor is Unit unit)
                UnitsController.ExecuteCommand(new RLBlockCommand<Node, Edge, Unit>
                    (unit), false);
        }
    }
}