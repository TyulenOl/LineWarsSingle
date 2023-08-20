using System.Diagnostics.CodeAnalysis;
using LineWars.Model;
using UnityEngine;


namespace LineWars
{
    public class UnitsController : MonoBehaviour
    {
        public static UnitsController Instance { get; private set; }
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
                    Debug.Log($"<color=yellow>{currentCommandIndex})</color> {command.GetLog()}");
                }
                command.Execute();
            }
        }
        
        public bool Action(IExecutor executor, ITarget target)
        {
            foreach (var commandType in target.CommandPriorityData.Priority)
            {
                if (commandType == CommandType.Attack &&
                    executor is IAttackerVisitor attacker && target is IAlive alive && attacker.CanAttack(alive))
                {
                    _ExecuteCommand(new AttackCommand(attacker, alive));
                    return true;
                }
                else if (commandType == CommandType.Move &&
                         (executor is IMovable movable && target is Node node && movable.CanMoveTo(node)))
                {
                    _ExecuteCommand(new MoveCommand(movable, movable.Node, node));
                    return true;
                }
                else if (commandType == CommandType.Heal &&
                         executor is Doctor doctor && target is Unit unit && doctor.CanHeal(unit))
                {
                    _ExecuteCommand(new HealCommand(doctor, unit));
                }
            }

            return false;
        }
    }
}