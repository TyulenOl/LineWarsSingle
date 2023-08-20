using UnityEngine;

namespace LineWars.Model
{
    public class DivideIntModifier: IntModifier
    {
        [SerializeField] private int divideValue;
        public override int Modify(int points)
        {
            return points / divideValue;
        }
    }
}