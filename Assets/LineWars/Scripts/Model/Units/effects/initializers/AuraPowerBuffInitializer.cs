using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "Aura Power Buff", menuName = "UnitEffects/Power Buff")]
    public class AuraPowerBuffInitializer : EffectInitializer
    {
        public override Effect<Node, Edge, Unit> GetEffect(Unit unit)
        {
            return new AuraPowerBuffEffect<Node, Edge, Unit>(unit);
        }
    }
}
