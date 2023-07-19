using System;
using System.Collections;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public enum CommandsState
    {
        Before, //на этом этапе должен выбраться приказ из возможных, и должен взяться исполнитель приказа
        Preparing, // на этом этапе должны отображаться объекты для взаимодействия, и должен выбраться то на что действует приказ
        Ready //на этом этапе происходит непосредственное выполнение приказа 
    }
    // Client
    public class CommandsManager: MonoBehaviour
    {
        public CommandsManager Instance { get; private set; }
        
        [Header("Debug")]
        [SerializeField] private bool isActive;
        
        private CommandsState currentState;
        
        private void Awake()
        {
            Instance = this;
            currentState = CommandsState.Before;
        }

        private void OnEnable()
        {
            //Selector.SelectedObjectsChanged += SelectorOnSelectedObjectsChanged;
        }

        private void SelectorOnSelectedObjectsChanged(GameObject before, GameObject after)
        {
            if (!isActive) return;
            
            switch (currentState)
            {
                case CommandsState.Before:
                    OnBeforeCommandState();
                    break;
                case CommandsState.Preparing:
                    OnPreparingCommandState();
                    break;
                case CommandsState.Ready:
                    OnReadyCommandState();
                    break;
            }
        }


        private void OnBeforeCommandState()
        {
        }

        private void OnPreparingCommandState()
        {
            
        }
        
        private void OnReadyCommandState()
        {
            
        }
    }
}