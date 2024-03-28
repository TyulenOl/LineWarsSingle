using System.Collections.Generic;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars
{
    public class NoReselectPreset : MonoBehaviour
    {
        [SerializeField] private List<ChooseCommandsButton> buttons;
        public void ReDraw(OnWaitingCommandMessage message)
        {
            gameObject.SetActive(true);
            for (var i = 0; i < buttons.Count; i++)
            {
                buttons[i].ReDraw(message.Data[i]);
            }
        }
    }
}