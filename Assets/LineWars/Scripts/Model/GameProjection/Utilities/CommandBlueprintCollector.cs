using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    public static class CommandBlueprintCollector
    {
        public static List<ICommandBlueprint> CollectAllCommands(GameProjection gameProjection)
        {
            var commands = new List<ICommandBlueprint>();
            switch (gameProjection.CurrentPhase)
            {
                case PhaseType.Buy:
                    CollectBuyState(commands, gameProjection);
                    break;
                case PhaseType.Artillery:
                    CollectArtilleryState(commands, gameProjection);
                    break;
                case PhaseType.Fight:
                    CollectFightState(commands, gameProjection);
                    break;
                case PhaseType.Scout:
                    CollectScoutState(commands, gameProjection);
                    break;
                default:
                    throw new ArgumentException("");
            }

            return commands;
        }

        private static void CollectBuyState(List<ICommandBlueprint> commands, GameProjection gameProjection)
        {
            var currentPlayer = gameProjection.CurrentPlayer;
            foreach(var preset in currentPlayer.EconomicLogic)
            {
                var newCommand = new SpawnPresetCommandBlueprint(currentPlayer.Id, preset);
                commands.Add(newCommand);
            }

        }

        private static void CollectArtilleryState(List<ICommandBlueprint> commands, GameProjection gameProjection)
        {
            CollectUsualState(PhaseType.Artillery, commands, gameProjection);
        }

        private static void CollectFightState(List<ICommandBlueprint> commands, GameProjection gameProjection)
        {
            CollectUsualState(PhaseType.Fight, commands, gameProjection);
        }

        private static void CollectScoutState(List<ICommandBlueprint> commands, GameProjection gameProjection)
        {
            CollectUsualState(PhaseType.Scout, commands, gameProjection);
        }

        private static void CollectUsualState(PhaseType type, List<ICommandBlueprint> commands, GameProjection gameProjection)
        {
            foreach (var unit in gameProjection.UnitsIndexList.Values)
            {
                if (unit.Owner != gameProjection.CurrentPlayer) continue;
                if (!gameProjection.CurrentPlayer.PhaseExecutorsData[type].Contains(unit.Type)) continue;

                ProcessUnit(commands, unit, gameProjection);
            }
        }

        private static void ProcessUnit(List<ICommandBlueprint> commands, UnitProjection projection, 
            GameProjection gameProjection)
        {
            foreach(var action in projection.ActionsDictionary.Values)
            {
                var visitor = ConvertUnitActionToBlueprints.Create(gameProjection.Graph, commands);
                action.Accept(visitor);
            }
        }
    }
}
