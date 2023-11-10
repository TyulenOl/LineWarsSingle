using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars
{
    public class ChooseCommandsCanvasPreset : MonoBehaviour
    {
        [SerializeField] private List<ChooseCommandsButton> buttons;
        public void ReDraw(OnWaitingCommandMessage message)
        {
            gameObject.SetActive(true);
            if (message.Data.Count() != buttons.Count)
                throw new InvalidOperationException(
                    $"Количество кнопок в окне канваса ({message.Data.Count()}) не совпадает с количеством команд ({buttons.Count})");
            var data = message.Data.ToArray();
            
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].ReDraw(data[i]);
            }
        }
    }
}
