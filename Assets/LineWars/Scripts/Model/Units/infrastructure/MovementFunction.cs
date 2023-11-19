using UnityEngine;

namespace LineWars.Model
{
    public abstract class MovementFunction : ScriptableObject
    {
        public abstract float Calculate(float value);
    }
}