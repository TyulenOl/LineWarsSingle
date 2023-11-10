using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;

namespace LineWars.Controllers
{
    public class GetAllAvailableTargetActionInfoForExecutorVisitor :
        IExecutorVisitor<IEnumerable<TargetActionInfo>>
    {
        private readonly GetAvailableTargetActionInfoForShotUnitAction forShotUnitAction;
        public GetAllAvailableTargetActionInfoForExecutorVisitor(
            GetAvailableTargetActionInfoForShotUnitAction forShotUnitAction)
        {
            this.forShotUnitAction = forShotUnitAction;
        }
        
        public IEnumerable<TargetActionInfo> Visit(Unit unit)
        {
            if (!unit.CanDoAnyAction)
                return Enumerable.Empty<TargetActionInfo>();
            return unit.Actions
                .SelectMany(x => x.Accept(new GetAvailableTargetActionInfoVisitor(forShotUnitAction)));
        }

        public IEnumerable<TargetActionInfo> Visit(UnitProjection unitProjection)
        {
            return Enumerable.Empty<TargetActionInfo>();;
        }
    }
}