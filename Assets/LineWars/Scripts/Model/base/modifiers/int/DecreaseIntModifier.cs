using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Decrease", menuName = "Modifiers/IntModifier/Decrease", order = 52)]
    public class DecreaseIntModifier : IntModifier
    {
        [SerializeField] private int decreaseValue;
        public override int Modify(int points)
        {
            return points - decreaseValue;
        }

        public static DecreaseIntModifier DecreaseOne
        {
            get
            {
                var value = CreateInstance<DecreaseIntModifier>();
                value.decreaseValue = 1;
                return value;
            }
        }
    }
}
