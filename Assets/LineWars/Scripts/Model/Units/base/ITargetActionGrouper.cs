using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    public interface ITargetActionGrouper
    {
        public Dictionary<Type, ITargetedAction[]> GroupByType(IEnumerable<ITargetedAction> targetedActions);
    }
}