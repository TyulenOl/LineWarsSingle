using System;
using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public class GetAllAvailableCommandsVisitor :
        IExecutorVisitor<(ITarget, ICommandWithCommandType[])[]>
    {
        public (ITarget, ICommandWithCommandType[])[] Visit(Unit unit)
        {
            return GetAllAvailableTargetsInRange(unit.MaxPossibleActionRadius + 1)
                .ToArray();

            IEnumerable<(ITarget, ICommandWithCommandType[])> GetAllAvailableTargetsInRange(uint range)
            {
                var actionsDictionary = unit.TargetTypeActionsDictionary;
                return GetAllPossibleTargetsInRange(range)
                    .Select(target =>
                    {
                        var actions = actionsDictionary[target.GetType()];
                        var commands = actions
                            .Select(targetedAction => targetedAction.GenerateCommand(target))
                            .ToArray();
                        return (target, commands);
                    });
            }

            IEnumerable<ITarget> GetAllPossibleTargetsInRange(uint range)
            {
                return MonoGraph.Instance.GetNodesInRange(unit.Node, range)
                    .SelectMany(node => node.Targets)
                    .Where(target => unit.PossibleTargetsTypes.Any(type => type.IsInstanceOfType(target)));
            }
        }

        public (ITarget, ICommandWithCommandType[])[] Visit(UnitProjection unitProjection)
        {
            throw new System.NotImplementedException();
        }
    }
}