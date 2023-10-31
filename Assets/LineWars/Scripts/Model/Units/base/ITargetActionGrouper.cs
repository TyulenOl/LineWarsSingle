using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    public interface ITargetActionGrouper
    {
        public TargetTypeActionsDictionary GroupByType(IEnumerable<ITargetedAction> targetedActions);
    }
}