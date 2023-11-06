using System;
using UnityEngine;

namespace LineWars.Model
{
    public abstract class FloatModifier: ScriptableObject
    {
        public abstract float Modify(float value);
    }
}