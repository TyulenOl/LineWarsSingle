using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class MoveActionProjection : UnitActionProjection
    {
        public MoveActionProjection(ComponentUnit.UnitAction action) : base(action)
        {
        }

        public MoveActionProjection(IReadOnlyUnitActionProjection projection) : base(projection)
        {
        }

        public override void ProjectAllOutcomes(List<GameProjection> list, IReadOnlyGameProjection projection, ComponentUnit unit)
        {
            ActionProjectionUtilities.IsUnitValidForAction(projection, this, unit);
            var readOnlyUnit = projection.GraphProjection.AllUnits[unit];

           
        }
  
    }
}
