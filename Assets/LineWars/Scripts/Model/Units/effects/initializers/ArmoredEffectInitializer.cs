using LineWars.Model;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Armored Effect", menuName = "UnitEffects/Armored")]
    public class ArmoredEffectInitializer : EffectInitializer
    {
        public override EffectType EffectType => EffectType.Armored;

        public override Effect<Node, Edge, Unit> GetEffect(Unit unit)
        {
            return new ArmoredEffect<Node, Edge, Unit>(unit);
        }
    }
}
