using System;
using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public class BlockActionProjection : UnitActionProjection, IReadOnlyBlockActionProjection
    {
        public bool Protection { get; set; }

        public bool IsBlocked { get; set; }

        public BlockActionProjection(ComponentUnit.BlockAction action) : base(action)
        {
            Protection = action.Protection;
            IsBlocked = action.IsBlocked;
        }

        public BlockActionProjection(IReadOnlyBlockActionProjection projection) : base(projection)
        {
            Protection = projection.Protection;
            IsBlocked = projection.IsBlocked;
        }

        public override void ProjectAllOutcomes(List<GameProjection> list, IReadOnlyGameProjection projection, ComponentUnit unit)
        {
            ActionProjectionUtilities.IsUnitValidForAction(projection, this, unit);
            var readOnlyUnit = projection.GraphProjection.AllUnits[unit];
            if (readOnlyUnit.CurrentActionPoints > 0)
            {
                var newProjection = new GameProjection(projection);
                var unitProjection = newProjection.Graph.Units[unit];
                unitProjection.CurrentActionPoints = 0;
                IsBlocked = true;
                list.Add(newProjection);
            }    
        }
    }

    public interface IReadOnlyBlockActionProjection : IReadOnlyUnitActionProjection
    {
        public bool Protection { get; }
        public bool IsBlocked { get; }
    }
}
