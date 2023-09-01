using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Multiply", menuName = "Modifiers/IntModifier/Multiply", order = 57)]
    public class MultiplyIntModifier: IntModifier
    {
        [SerializeField] private float multiplyValue = 1; 
        public override int Modify(int points)
        {
            return Mathf.RoundToInt(points * multiplyValue);
        }
        
        public static MultiplyIntModifier MultiplyOne
        {
            get
            {
                var value = CreateInstance<MultiplyIntModifier>();
                value.multiplyValue = 1;
                return value;
            }
        }
    }
}