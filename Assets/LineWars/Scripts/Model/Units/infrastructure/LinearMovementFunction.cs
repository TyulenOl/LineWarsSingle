using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "Movement Functions/Linear", order = 70)]
    public class LinearMovementFunction: MovementFunction
    {
        public override float Calculate(float value)
        {
            return value;
        }
    }
}