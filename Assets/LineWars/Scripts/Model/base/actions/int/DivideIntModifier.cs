using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Divide", menuName = "Modifiers/IntModifier/Divide", order = 52)]
    public class DivideIntModifier: IntModifier
    {
        [SerializeField] private int divideValue;
        public override int Modify(int points)
        {
            return points / divideValue;
        }
    }
}