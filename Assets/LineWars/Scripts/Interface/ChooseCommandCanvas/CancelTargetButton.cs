using System;
using LineWars.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    [RequireComponent(typeof(Button))]
    public class CancelTargetButton : MonoBehaviour
    {
        [SerializeField] private ChooseCommandsCanvasPreset commandsCanvasPreset;
        private Button button;
        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener((() =>
            {
                CommandsManager.Instance.CancelTarget();
                commandsCanvasPreset.gameObject.SetActive(false);
            }));
        }
    }
}