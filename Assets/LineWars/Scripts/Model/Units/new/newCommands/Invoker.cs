using JetBrains.Annotations;
using UnityEngine;

namespace LineWars.Model
{
    public class Invoker: MonoBehaviour
    {
        public static Invoker Instance { get; private set; }
        [SerializeField] private bool needLog = true;  
        private int currentCommandIndex;

        private void Awake()
        {
            Instance = this;
        }

        public static void ExecuteCommand([NotNull]ICommand command, bool dontCheckExecute = true)
        {
            if (Instance != null)
                Instance._ExecuteCommand(command, dontCheckExecute);
        }

        private void _ExecuteCommand([NotNull] ICommand command, bool dontCheckExecute = true)
        {
            if (dontCheckExecute || command.CanExecute())
            {
                currentCommandIndex++;
                if (needLog)
                {
                    Debug.Log($"<color=yellow>COMMAND {currentCommandIndex}</color> {command.GetLog()}");
                }
                command.Execute();
            }
        }
        
        public bool Action(IExecutor executor, ITarget target)
        {
            foreach (var commandType in target.CommandPriorityData.Priority)
            {
                switch (commandType)
                {
                }
            }

            return false;
        }

        public CommandType GetCommandTypeBy(IExecutor executor, ITarget target)
        {
            foreach (var commandType in target.CommandPriorityData.Priority)
            {
                switch (commandType)
                {
                    
                }
            }

            return CommandType.None;
        }
    }
}