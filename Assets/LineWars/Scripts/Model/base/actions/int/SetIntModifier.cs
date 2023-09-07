using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Set", menuName = "Modifiers/IntModifier/Set", order = 52)]
    public class SetIntModifier : IntModifier
    {
        [SerializeField] private int value = 0;

        public override int Modify(int points)
        {
            return value;
        }

        public static SetIntModifier Set0 => CreateInstance<SetIntModifier>();
    }
}
