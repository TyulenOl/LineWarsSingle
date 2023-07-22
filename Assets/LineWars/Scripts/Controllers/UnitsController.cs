using System;
using System.Diagnostics.CodeAnalysis;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    // класс только для сервера
    public class  UnitsController : MonoBehaviour
    {
        public static UnitsController Instance { get; private set; }

        private Invoker invoker;

        private void Awake()
        {
            invoker = new Invoker();
            Instance = this;
        }

        public static void ExecuteCommand(ICommand command)
        {
            if (Instance != null)
                Instance.invoker.Execute(command);
        }
        
        public void Action([NotNull] Player owner, IExecutor executor, ITarget target)
        {
        }
        
    }
}