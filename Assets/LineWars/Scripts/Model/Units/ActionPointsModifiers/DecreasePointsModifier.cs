using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Decrease", menuName = "Action Points Modifier/Decrease")]
    public class DecreasePointsModifier : ActionPointsModifier
    {
        [SerializeField] private int decreaseValue;
        public override int Modify(int points)
        {
            return points - decreaseValue;
        }
    }
}
