using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class ChooseCommandsButton : MonoBehaviour
    {
        [SerializeField] private Image commandImage;
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text costOfAction;
        
        private CommandPreset hash;
        private ChooseCommandsCanvasPreset preset;
        
        private void Awake()
        {
            button.onClick.AddListener(OnButtonClick);
            preset = GetComponentInParent<ChooseCommandsCanvasPreset>();
        }

        public void ReDraw(CommandPreset commandPreset)
        {
            hash = commandPreset;
            var sprite = DrawHelper.GetIconByCommandType(hash.Action.CommandType);
            commandImage.sprite = sprite;
            if(costOfAction != null)
                costOfAction.text = commandPreset.Action.GetActionPointsCost().ToString();
        }

        private void OnButtonClick()
        {
            if (CommandsManager.Instance.SelectCommandsPreset(hash))
            {
                preset.gameObject.SetActive(false);
            }
        }
    }
}
