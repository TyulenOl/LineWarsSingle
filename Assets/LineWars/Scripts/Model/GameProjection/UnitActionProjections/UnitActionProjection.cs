using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    public abstract class UnitActionProjection : IReadOnlyUnitActionProjection
    {
        public ComponentUnit.UnitAction Action { get; private set; }
        protected UnitActionProjection(ComponentUnit.UnitAction action)
        {
            Action = action;
        }
        public abstract void ProjectAllOutcomes(List<GameProjection> list, IReadOnlyGameProjection projection, ComponentUnit unit);

        protected UnitActionProjection(IReadOnlyUnitActionProjection projection)
        {
            Action = projection.Action;
        }

        #region Registry
        public static UnitActionProjection GetActionProjection(ComponentUnit.UnitAction action)
        {
            throw new ArgumentException("Action is not registered!");
        }

        public static UnitActionProjection GetActionProjection(IReadOnlyUnitActionProjection projection)
        {
            throw new ArgumentException("Action is not registered!");
        }

        #region Block Action
        public static UnitActionProjection GetActionProjection(ComponentUnit.BlockAction action)
        {
            return new BlockActionProjection(action);
        }

        public static UnitActionProjection GetActionProjection(IReadOnlyBlockActionProjection projection)
        {
            return new BlockActionProjection(projection);
        }
        #endregion
        #endregion
    }

    public interface IReadOnlyUnitActionProjection
    {
        public ComponentUnit.UnitAction Action { get;}
    }
}

