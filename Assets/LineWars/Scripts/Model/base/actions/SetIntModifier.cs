using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Set", menuName = "IntModifier/Set", order = 52)]
    public class SetIntModifier : IntModifier
    {
        [SerializeField] private int value;

        public override int Modify(int points)
        {
            return value;
        }
    }
}
