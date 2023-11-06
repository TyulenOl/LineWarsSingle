using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public class AllAvailableCommandForExecutorVisitor :
        IExecutorVisitor<IEnumerable<(ITarget, IEnumerable<ICommandWithCommandType>)>>
    {
        public IEnumerable<(ITarget, IEnumerable<ICommandWithCommandType>)> Visit(Unit unit)
        {
            if (!unit.CanDoAnyAction)
                return Enumerable.Empty<(ITarget, IEnumerable<ICommandWithCommandType>)>();
            return unit.Actions
                .SelectMany(action =>
                    action.Accept(new AllCommandsVisitor<Node, Edge, Unit>(MonoGraph.Instance)))
                .Where(context => context.Command.CanExecute())
                .GroupBy(
                    context => context.Target,
                    context => context.Command,
                    (target, contexts) => (target, contexts));
        }

        public IEnumerable<(ITarget, IEnumerable<ICommandWithCommandType>)> Visit(UnitProjection unitProjection)
        {
            return Enumerable.Empty<(ITarget, IEnumerable<ICommandWithCommandType>)>();;
        }
    }
}