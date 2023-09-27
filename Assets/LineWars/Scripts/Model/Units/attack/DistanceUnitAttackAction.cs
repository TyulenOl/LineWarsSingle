using System;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    //[CreateAssetMenu(fileName = "New DistanceAttack", menuName = "UnitActions/Attack/DistanceAttack", order = 61)]
    public class DistanceUnitAttackAction: BaseUnitAttackAction
    {
        [field: SerializeField, Min(0)] public int Distance { get; private set; }
        public override UnitAction GetAction(ComponentUnit unit) => new DistanceAttackAction(unit, this);
    }
}