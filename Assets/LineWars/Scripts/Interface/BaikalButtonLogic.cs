using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class BaikalButtonLogic : MonoBehaviour
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
            {
                unit.CurrentHp += 1;
                unit.CurrentActionPoints = 0;
            }
            //TODO переписать на команду
        }
    }
}