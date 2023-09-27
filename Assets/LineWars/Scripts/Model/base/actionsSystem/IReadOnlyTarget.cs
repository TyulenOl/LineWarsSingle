using UnityEngine;
using System.Collections.Generic;

namespace LineWars.Model
{
    public interface IReadOnlyTarget
    {
        public CommandPriorityData CommandPriorityData {get;}
    }
}

