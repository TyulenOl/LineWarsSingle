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
        [SerializeField] private ChooseCommandsCanvasPreset forOnePreset;
        [SerializeField] private ChooseCommandsCanvasPreset forTwoPreset;
        [SerializeField] private ChooseCommandsCanvasPreset forThreePreset;
        [SerializeField] private ChooseCommandsCanvasPreset forFourPreset;

        private void Awake()
        {
            CommandsManager.Instance.InWaitingCommandState += OnWaitingState;
        }

        private void OnWaitingState(OnWaitingCommandMessage message)
        {
            gameObject.SetActive(true);
            transform.position = message.SelectedNode.Position;
            
            switch (message.Data.Count())
            {
                case 1:
                    forOnePreset.ReDraw(message);
                    break;
                case 2:
                    forTwoPreset.ReDraw(message);
                    break;
                case 3:
                    forThreePreset.ReDraw(message);
                    break;
                case 4:
                    forFourPreset.ReDraw(message);
                    break;
                default:
                    Debug.LogError($"Окно выбра команд для количества аргументов {message.Data.Count()} не реализовано");
                    break;
            }
        }
    }
}