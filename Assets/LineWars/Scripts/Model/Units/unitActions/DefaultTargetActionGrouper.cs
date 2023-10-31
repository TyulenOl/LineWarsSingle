using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace LineWars.Model
{
    public class DefaultTargetActionGrouper : ITargetActionGrouper
    {
        public TargetTypeActionsDictionary GroupByType(IEnumerable<ITargetedAction> targetedActions)
        {
            return new TargetTypeActionsDictionary(targetedActions.GroupBy(action => action.TargetType,
                    action => action,
                    (targetType, actions) => (targetType, actions.ToArray()))
                .Select(x => new KeyValuePair<Type,ITargetedAction[]>(x.targetType, x.Item2)));
        }
    }
}