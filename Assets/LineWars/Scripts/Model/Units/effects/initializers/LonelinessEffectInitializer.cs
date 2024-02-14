using System;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "Fighting Spirit", menuName = "UnitEffects/Loneliness")]
    public class LonelinessEffectInitializer : EffectInitializer
    {
        [SerializeField] private int powerBonus;

        public override EffectType EffectType => EffectType.Loneliness;

        public override Effect<Node, Edge, Unit> GetEffect(Unit unit)
        {
            return new LonelinessEffect<Node, Edge, Unit>(unit, powerBonus);
        }
    }
}
