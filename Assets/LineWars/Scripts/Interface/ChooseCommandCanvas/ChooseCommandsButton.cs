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
        
        private (ITarget, IActionCommand) hash;
        private ChooseCommandsCanvas canvas;
        
        private void Awake()
        {
            button.onClick.AddListener(OnButtonClick);
            canvas = GetComponentInParent<ChooseCommandsCanvas>();
        }

        public void ReDraw((ITarget, IActionCommand) tuple)
        {
            hash = tuple;
            commandImage.sprite = DrawHelper.GetSpriteByCommandType(tuple.Item2.CommandType);
        }

        private void OnButtonClick()
        {
            CommandsManager.Instance.SelectCommand(hash.Item2);
            canvas.gameObject.SetActive(false);
        }
    }
}
