using System;
using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public class DefaultTargetActionGrouper : ITargetActionGrouper
    {
        public Dictionary<Type, ITargetedAction[]> GroupByType(IEnumerable<ITargetedAction> targetedActions)
        {
            return targetedActions
                .GroupBy(action => action.TargetType,
                    action => action,
                    (targetType, actions) => (targetType, actions.ToArray()))
                .ToDictionary(
                    group => group.Item1,
                    group => group.Item2);
        }
    }
}