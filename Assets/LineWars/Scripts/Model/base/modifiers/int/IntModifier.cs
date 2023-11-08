using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public abstract class IntModifier : ScriptableObject
    {
        public abstract int Modify(int points);
    }
}
