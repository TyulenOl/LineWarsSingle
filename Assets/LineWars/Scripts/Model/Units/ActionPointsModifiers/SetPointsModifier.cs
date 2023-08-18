using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Set", menuName = "Action Points Modifier/Set")]
    public class SetPointsModifier : ActionPointsModifier
    {
        [SerializeField] private int value;

        public override int Modify(int points)
        {
            return value;
        }
    }
}
