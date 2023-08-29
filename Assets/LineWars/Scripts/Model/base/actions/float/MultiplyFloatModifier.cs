using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Multiply", menuName = "Modifiers/FloatModifier/Multiply", order = 50)]
    public class MultiplyFloatModifier: FloatModifier
    {
        [SerializeField] private float multiplyValue;
        
        public override float Modify(float value)
        {
            return value * multiplyValue;
        }

        protected override void Awake()
        {
            multiplyValue = 1;
        }
    }
}