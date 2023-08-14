using System;
using System.Diagnostics.CodeAnalysis;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    // класс только для сервера
    public class UnitsController : MonoBehaviour
    {
        public static UnitsController Instance { get; private set; }

        private Invoker invoker;

        private void Awake()
        {
            invoker = new Invoker();
            Instance = this;
        }

        public static void ExecuteCommand(ICommand command, bool dontCheckExecute = true)
        {
            if (Instance != null && (dontCheckExecute || command.CanExecute()))
                Instance.invoker.Execute(command);
        }

        public void Action(IExecutor executor, ITarget target)
        {
            Debug.Log("ACTION");

            foreach (var commandType in target.CommandPriorityData.Priority)
            {
                if (commandType == CommandType.Attack &&
                    (executor is IAttackerVisitor attacker && target is IAlive alive && attacker.CanAttack(alive)))
                {
                    invoker.Execute(new AttackCommand((IAttackerVisitor) executor, (IAlive) target));
                    Debug.Log("Attack performed");
                    break;
                }
                else if (commandType == CommandType.Move &&
                         (executor is IMovable movable && target is Node node && movable.CanMoveTo(node)))
                {
                    invoker.Execute(new MoveCommand(movable, movable.Node, (Node) target));
                    Debug.Log("Movement Performed");
                    break;
                }
            }
        }
    }
}