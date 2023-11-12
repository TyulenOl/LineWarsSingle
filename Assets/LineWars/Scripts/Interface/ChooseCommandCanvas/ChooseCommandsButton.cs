using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class ChooseCommandsButton : MonoBehaviour
    {
        [SerializeField] private Image commandImage;
        [SerializeField] private Button button;
        
        private CommandPreset hash;
        private ChooseCommandsCanvasPreset preset;
        
        private void Awake()
        {
            button.onClick.AddListener(OnButtonClick);
            preset = GetComponentInParent<ChooseCommandsCanvasPreset>();
        }

        public void ReDraw(CommandPreset tuple)
        {
            hash = tuple;
            var sprite = DrawHelper.GetSpriteByCommandType(hash.Action.CommandType);
            commandImage.sprite = sprite;
        }

        private void OnButtonClick()
        {
            CommandsManager.Instance.SelectCommandsPreset(hash);
            preset.gameObject.SetActive(false);
        }
    }
}
