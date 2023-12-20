using System;
using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public static class CommandBlueprintCollector
    {
        public static List<ICommandBlueprint> CollectAllCommands(
            IReadOnlyGameProjection gameProjection,
            IReadOnlyCollection<CommandType> availableCommands)
        {
            var commands = new List<ICommandBlueprint>();
            switch (gameProjection.CurrentPhase)
            {
                case PhaseType.Buy:
                    CollectBuyState(commands, gameProjection, availableCommands);
                    break;
                case PhaseType.Artillery:
                    CollectArtilleryState(commands, gameProjection, availableCommands);
                    break;
                case PhaseType.Fight:
                    CollectFightState(commands, gameProjection, availableCommands);
                    break;
                case PhaseType.Scout:
                    CollectScoutState(commands, gameProjection, availableCommands);
                    break;
                default:
                    throw new ArgumentException("");
            }

            return commands;
        }

        private static void CollectBuyState(
            List<ICommandBlueprint> commands, 
            IReadOnlyGameProjection gameProjection,
            IReadOnlyCollection<CommandType> availableCommands)
        {
            var currentPlayer = gameProjection.CurrentPlayer;
            foreach(var preset in currentPlayer.EconomicLogic)
            {
                var newCommand = new SpawnPresetCommandBlueprint(currentPlayer.Id, preset);
                commands.Add(newCommand);
            }

        }

        private static void CollectArtilleryState(
            List<ICommandBlueprint> commands, 
            IReadOnlyGameProjection gameProjection,
            IReadOnlyCollection<CommandType> availableCommands)
        {
            CollectUsualState(PhaseType.Artillery, commands, gameProjection, availableCommands);
        }

        private static void CollectFightState(
            List<ICommandBlueprint> commands, 
            IReadOnlyGameProjection gameProjection,
            IReadOnlyCollection<CommandType> availableCommands)
        {
            CollectUsualState(PhaseType.Fight, commands, gameProjection, availableCommands);
        }

        private static void CollectScoutState(
            List<ICommandBlueprint> commands,
            IReadOnlyGameProjection gameProjection,
            IReadOnlyCollection<CommandType> availableCommands)
        {
            CollectUsualState(PhaseType.Scout, commands, gameProjection, availableCommands);
        }

        private static void CollectUsualState(
            PhaseType type, 
            List<ICommandBlueprint> commands, 
            IReadOnlyGameProjection gameProjection,
            IReadOnlyCollection<CommandType> availableCommands)
        {
            var units = gameProjection.CurrentPlayer.OwnedObjects
                .OfType<UnitProjection>()
                .ToList();
            foreach (var unit in units)
            {
                if (unit.Owner != gameProjection.CurrentPlayer) continue;
                if (!gameProjection.CurrentPlayer.PhaseExecutorsData[type].Contains(unit.Type)) continue;
                ProcessUnit(commands, unit, gameProjection, availableCommands);
            }
        }

        private static void ProcessUnit(
            List<ICommandBlueprint> commands,
            IReadOnlyUnitProjection projection,
            IReadOnlyGameProjection gameProjection,
            IReadOnlyCollection<CommandType> availableCommands)
        {
            foreach(var action in projection.ActionsDictionary.Values)
            {
                if (!availableCommands.Contains(action.CommandType)) continue;
                var visitor = ConvertUnitActionToBlueprints.Create(gameProjection.Graph, commands);
                action.Accept(visitor);
            }
        }
    }
}
