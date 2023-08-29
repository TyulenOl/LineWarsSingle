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
    }
}
