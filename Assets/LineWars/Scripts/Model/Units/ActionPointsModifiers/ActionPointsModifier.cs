using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public abstract class ActionPointsModifier : ScriptableObject
    {
        public abstract int Modify(int points);
    }
}
