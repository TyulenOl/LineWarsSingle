using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;

namespace LineWars.Controllers
{
    public class GetAllAvailableTargetActionInfoForMonoExecutorVisitor :
        IMonoExecutorVisitor<IEnumerable<TargetActionInfo>>
    {
        private readonly GetAvailableTargetActionInfoVisitor.ForShotUnitAction forShotUnitAction;
        private readonly Func<IUnitAction<Node, Edge, Unit>, bool> actionSelector;
        public GetAllAvailableTargetActionInfoForMonoExecutorVisitor(
            GetAvailableTargetActionInfoVisitor.ForShotUnitAction forShotUnitAction, 
            Func<IUnitAction<Node, Edge, Unit>, bool> actionSelector = null)
        {
            this.forShotUnitAction = forShotUnitAction;
            this.actionSelector = actionSelector ?? (_ => true);
        }

        public IEnumerable<TargetActionInfo> Visit(Unit unit)
        {
            if (!unit.CanDoAnyAction)
                return Enumerable.Empty<TargetActionInfo>();
            return unit.Actions
                .Where(actionSelector)
                .SelectMany(x => x.Accept(
                    new GetAvailableTargetActionInfoVisitor(forShotUnitAction))
                );
        }
    }
}