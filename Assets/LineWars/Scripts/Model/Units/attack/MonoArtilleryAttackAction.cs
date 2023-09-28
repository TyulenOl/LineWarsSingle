using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoArtilleryAttackAction : MonoDistanceAttackAction
    {
        [SerializeField] private Explosion explosionPrefab;
        protected override UnitAction GetAction(ComponentUnit unit) => new ArtilleryAttackAction(unit, this);
    }
}