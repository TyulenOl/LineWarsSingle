using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "Movement Functions/Linear")]
    public class LinearMovementFunction: MovementFunction
    {
        public override float Calculate(float value)
        {
            return value;
        }
    }
}