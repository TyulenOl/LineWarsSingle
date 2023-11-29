using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars
{
    public class ChooseCommandsCanvas : MonoBehaviour
    {
        [SerializeField] private ChooseCommandsCanvasPreset forTwoPreset;
        [SerializeField] private ChooseCommandsCanvasPreset forThreePreset;

        private void Awake()
        {
            CommandsManager.Instance.InWaitingCommandState += OnWaitingState;
        }

        private void OnWaitingState(OnWaitingCommandMessage message)
        {
            gameObject.SetActive(true);
            transform.position = message.SelectedNode.Position;
            var amount = message.Data.Select(x=> x.Action.CommandType).Distinct().Count();
            
            var presets = message.Data.GroupBy(x => x.Action.CommandType).ToArray();

            var message2 = new OnWaitingCommandMessage(presets.Select(x => x.ToArray().First()), message.SelectedNode);
            
            switch (message2.Data.Count())
            {
                case 2:
                    forTwoPreset.ReDraw(message2);
                    break;
                case 3:
                    forThreePreset.ReDraw(message2);
                    break;
                default:
                    throw new NotImplementedException(
                        $"Окно выбра команд для количества аргументов {message2.Data.Count()} не реализовано");
            }
        }
    }
}