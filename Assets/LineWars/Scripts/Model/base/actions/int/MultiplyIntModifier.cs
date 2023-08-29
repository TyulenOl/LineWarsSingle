using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Multiply", menuName = "Modifiers/IntModifier/Multiply", order = 57)]
    public class MultiplyIntModifier: IntModifier
    {
        [SerializeField] private float multiplyValue; 
        public override int Modify(int points)
        {
            return Mathf.RoundToInt(points * multiplyValue);
        }

        protected override void Awake()
        {
            multiplyValue = 1;
        }
    }
}