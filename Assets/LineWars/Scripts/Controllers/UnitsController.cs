using System;
using System.Collections.Generic;
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
                    case CommandType.MeleeAttack when
                        executor is IAttackerVisitor attacker && target is IAlive alive && attacker.CanAttack(alive):
                        UnitsController.ExecuteCommand(new UnitAttackCommand(attacker, alive));
                        return true;
                    case CommandType.Move when
                        (executor is IMovable movable && target is Node node && movable.CanMoveTo(node)):
                        UnitsController.ExecuteCommand(new UnitMoveCommand(movable, movable.Node, node));
                        return true;
                    case CommandType.Heal when
                        executor is Doctor doctor && target is ComponentUnit unit && doctor.CanHeal(unit):
                        UnitsController.ExecuteCommand(new UnitHealCommand(doctor, unit));
                        return true;
                    case CommandType.Build 
                         when executor is Engineer engineer && target is Edge edge && engineer.CanUpRoad(edge):
                        UnitsController.ExecuteCommand(new UnitUpRoadCommand(engineer, edge));
                        return true;
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
                    case CommandType.MeleeAttack when
                        executor is IAttackerVisitor attacker && target is IAlive alive && attacker.CanAttack(alive):
                        return attacker.GetAttackTypeBy(alive);
                    case CommandType.Move when
                        (executor is IMovable movable && target is Node node && movable.CanMoveTo(node)):
                        return CommandType.Move;
                    case CommandType.Heal when
                        executor is Doctor doctor && target is ComponentUnit unit && doctor.CanHeal(unit):
                        return CommandType.Heal;
                    case CommandType.Build 
                        when executor is Engineer engineer && target is Edge edge && engineer.CanUpRoad(edge):
                        return CommandType.Build;
                }
            }

            return CommandType.None;
        }
    }
}