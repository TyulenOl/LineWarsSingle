using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars
{
    public class Education : MonoBehaviour
    {
        [SerializeField] private CommandsManager commandsManager;
        [SerializeField] private CameraController cameraController;

        private void Start()
        {
            commandsManager.Deactivate();
        }


#if UNITY_EDITOR
        [Header("DEBUG")]
        [SerializeField] private bool commandsManagerActiveSelf;
        private void Update()
        {
            if (commandsManagerActiveSelf != CommandsManager.Instance.ActiveSelf)
            {
                if (commandsManagerActiveSelf)
                    commandsManager.Activate();
                else
                    commandsManager.Deactivate();
            }
        }
#endif
    }
}