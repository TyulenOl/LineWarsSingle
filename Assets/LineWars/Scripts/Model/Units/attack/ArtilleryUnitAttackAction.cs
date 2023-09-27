using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class ArtilleryUnitAttackAction : DistanceUnitAttackAction
    {
        [SerializeField] private Explosion explosionPrefab;

        public Explosion ExplosionPrefab => explosionPrefab;

        public override UnitAction GetAction(ComponentUnit unit) => new ArtilleryAttackAction(unit, this);
    }
}