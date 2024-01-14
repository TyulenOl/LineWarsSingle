using UnityEngine;

namespace LineWars.Model
{
    public abstract class EffectInitializer : ScriptableObject
    {
        public abstract Effect<Node, Edge, Unit> GetEffect(Unit unit);
    }
}
