using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "Fighting Spirit", menuName = "UnitEffects/Fighting Spirit")]
    public class FightingSpiritEffectInitializer : EffectInitializer
    {
        [SerializeField] private int powerBonus;
        public override Effect<Node, Edge, Unit> GetEffect(Unit unit)
        {
            return new FightingSpiritEffect<Node, Edge, Unit>(unit, powerBonus);
        }
    }
}
