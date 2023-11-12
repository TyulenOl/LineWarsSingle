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
            var amount = message.Data.Count();
            switch (amount)
            {
                case 2:
                    forTwoPreset.ReDraw(message);
                    break;
                case 3:
                    forThreePreset.ReDraw(message);
                    break;
                default:
                    throw new NotImplementedException(
                        $"Окно выбра команд для количества аргументов {amount} не реализовано");
            }
        }
    }
}