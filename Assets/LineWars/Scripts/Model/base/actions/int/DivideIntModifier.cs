using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Divide", menuName = "Modifiers/IntModifier/Divide", order = 52)]
    public class DivideIntModifier: IntModifier
    {
        [SerializeField] private float divideValue = 2;
        public override int Modify(int points)
        {
            return Mathf.RoundToInt(points / divideValue);
        }
        
        public static DivideIntModifier DivideTwo
        {
            get
            {
                var value = CreateInstance<DivideIntModifier>();
                value.divideValue = 2;
                return value;
            }
        }
    }
}