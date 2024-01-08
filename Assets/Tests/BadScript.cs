// using System;
// using System.Collections;
// using LineWars.Controllers;
// using UnityEngine;
//
// namespace Tests
// {
//     public class BadScript: MonoBehaviour
//     {
//         [SerializeField] private bool activateCommandsManager;
//         private IEnumerator Start()
//         {
//             yield return null;
//             CommandsManager.Instance.ExecutorChanged +=
//                 (executor, monoExecutor) => CommandsManager.Instance.Deactivate();
//             activateCommandsManager = false;
//         }
//
//         // private void Update()
//         // {
//         //     if (activateCommandsManager != CommandsManager.Instance.ActiveSelf)
//         //     {
//         //         if (activateCommandsManager) 
//         //             CommandsManager.Instance.Activate();
//         //         else
//         //             CommandsManager.Instance.Deactivate();
//         //     }
//         // }
//     }
// }